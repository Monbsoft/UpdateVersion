using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.CommandLine;
using System.IO;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class VersionCommandArguments
    {
        public bool Add { get; set; }
        public string Message { get; set; }
        public bool Tag { get; set; }
        public IConsole Console { get; set; } = default;
        public Verbosity Verbosity { get; set; } = Verbosity.Info;
    }

    public abstract class VersionCommandBase
    {
        protected ProjectStore _store;
        protected IGitService _gitService;

        public VersionCommandBase(IGitService gitService)
        {
            _store = new ProjectStore();
            _gitService = gitService ?? throw new ArgumentNullException(nameof(gitService));
        }

        protected static Command CreateCommand(string name, string description)
        {
            var command = new Command(name, description)
            {
                new Option(new string[]{"--message", "-m" }, "Message of the git commit")
                {
                    Argument = new Argument<string>{  Name = "message" }
                },
                new Option(new string[]{"--add", "-a"}, "All files in the entire working tree"),
                new Option(new string[]{"--tag", "-t" }, "Tag of the git commit"),
                new Option(new string[]{"-v", "--verbosity"}, "Level of the verbosity")
                {
                    Argument = new Argument<Verbosity> { Name = "level" }
                }
            };
            return command;
        }

        protected async Task<bool> AddTagAsync(CommandContext context)
        {
            if(!await _gitService.IsInstalled())
                throw new InvalidOperationException("Unable to commit because git is not installed.");

            return await _gitService.RunCommandAsync(context, $"tag -a v{context.NewVesion} -m \"Version {context.NewVesion}\"");
        }

        protected async Task<bool> CommitAsync(CommandContext context)
        {

            if (!await _gitService.IsInstalled())
                throw new InvalidOperationException("Unable to commit because git is not installed.");

            if (context.Add)
                await _gitService.RunCommandAsync(context, $"add --all");

            return await _gitService.RunCommandAsync(context, $"commit -a -m \"{context.Message}\"");
        }

        protected static CommandContext  CreateCommandContext(VersionCommandArguments args)
        {
            return new CommandContext(args.Console, args.Verbosity)
            {
                Add = args.Add,
                Directory = Directory.GetCurrentDirectory(),
                Message = args.Message,
                Tag = args.Tag,
                Verbosity = args.Verbosity
            };
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
                context.NewVesion = UpdateProject(project, changeVersion);
                _store.Save(project);
            }

            if (!string.IsNullOrEmpty(context.Message))
            {
                if (!await CommitAsync(context))
                {
                    context.WriteWarning("Failed to commit.");
                    return projectFiles.Count;
                }
                context.WriteInfo($"Commit \"{context.Message}\" is created.");
            }

            if (context.Tag)
            {
                if (!await AddTagAsync(context))
                {
                    context.WriteWarning("Failed to add tag.");
                    return projectFiles.Count;
                }
                context.WriteInfo($"Tag v{context.NewVesion} is added");
            }
            return projectFiles.Count;
        }

        /// <summary>
        /// Updates the version of the project
        /// </summary>
        /// <param name="project"></param>
        /// <param name="changeVersion"></param>
        /// <returns></returns>
        protected string UpdateProject(Project project, Func<SemVersion, SemVersion> changeVersion)
        {
            var oldVersion = SemVersion.Parse(project.Version);
            var newVersion = changeVersion(oldVersion);
            project.Version = newVersion.ToString();
            return project.Version;
        }
    }
}