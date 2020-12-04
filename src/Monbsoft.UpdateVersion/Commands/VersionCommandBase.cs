using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.CommandLine;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class VersionCommandArguments
    {
        public string Message { get; set; }
        public IConsole Console { get; set; } = default;
        public Verbosity Verbosity { get; set; } = Verbosity.Info;
    }

    public abstract class VersionCommandBase
    {
        private ProjectStore _store;

        public VersionCommandBase()
        {
            _store = new ProjectStore();
        }

        protected static Command CreateCommand(string name, string description)
        {
            var command = new Command(name, description)
            {
                new Option(new string[]{"--message", "-m" }, "Message of the git commit")
                {
                    Argument = new Argument<string>{  Name = "message" }
                },
                new Option(new string[]{"-v", "--verbosity"}, "Level of the verbosity")
                {
                    Argument = new Argument<Verbosity> { Name = "level" }
                }
            };
            return command;
        }

        protected async Task<bool> CommitAsync(CommandContext context)
        {
            if (string.IsNullOrEmpty(context.Message))
                return false;

            if (!await GitUtils.IsInstalled())
                throw new InvalidOperationException("Unable to commit because git is not installed.");

            return await GitUtils.RunCommandAsync(context, $"commit -a -m \"{context.Message}\"");
        }

        /// <summary>
        /// Updates the versions of the projects.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="changeVersion"></param>
        /// <returns></returns>
        protected async Task<int> UpdateAsync(CommandContext context, Func<SemVersion, SemVersion> changeVersion)
        {
            context.WriteDebug("Updating versions...");
            var finder = new ProjectFinder(context.Directory);
            var projectFiles = finder.FindProjects();
            foreach (var projectFile in projectFiles)
            {
                var project = _store.Read(projectFile);
                UpdateProject(project, changeVersion);
                _store.Save(project);
            }

            bool result = await CommitAsync(context);
            if (!result)
            {
                context.WriteWarning("Failed to commit.");
                return projectFiles.Count;
            }

            context.WriteInfo($"Commit \"{context.Message}\" is created.");
            return projectFiles.Count;
        }

        /// <summary>
        /// Updates the version of the project.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="changeVersion"></param>
        protected void UpdateProject(Project project, Func<SemVersion, SemVersion> changeVersion)
        {
            var oldVersion = SemVersion.Parse(project.Version);
            var newVersion = changeVersion(oldVersion);
            project.Version = newVersion.ToString();
        }
    }
}