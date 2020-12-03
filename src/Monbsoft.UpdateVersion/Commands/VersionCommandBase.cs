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
        public string Message { get; set;  }
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
                new Option("--message", "Message of the git commit")
                {
                    Argument = new Argument<string>
                    {
                        Name = "message",
                    }
                }

            };
            return command;
        }

        protected async Task CommitAsync(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;

            if (!await GitUtils.IsInstalled())
                throw new InvalidOperationException("Unable to commit because git is not installed.");

            await GitUtils.RunCommandAsync($"commit -am \"{message}\"");
        }

        /// <summary>
        /// Updates the versions of the projects.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="changeVersion"></param>
        /// <returns></returns>
        protected async Task<int> UpdateAsync(CommandContext context, Func<SemVersion, SemVersion> changeVersion)
        {
            var finder = new ProjectFinder(context.Directory);
            var projectFiles = finder.FindProjects();
            foreach (var projectFile in projectFiles)
            {
                var project = _store.Read(projectFile);
                UpdateProject(project, changeVersion);
                _store.Save(project);
            }

            await CommitAsync(context.Message);

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