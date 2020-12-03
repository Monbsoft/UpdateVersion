using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.CommandLine;

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

        protected int Update(CommandContext context, Func<SemVersion, SemVersion> changeVersion)
        {
            var finder = new ProjectFinder(context.Directory);
            var projectFiles = finder.FindProjects();
            foreach (var projectFile in projectFiles)
            {
                var project = _store.Read(projectFile);
                UpdateProject(project, changeVersion);
                _store.Save(project);
            }
            return projectFiles.Count;
        }

        protected void UpdateProject(Project project, Func<SemVersion, SemVersion> changeVersion)
        {
            var oldVersion = SemVersion.Parse(project.Version);
            var newVersion = changeVersion(oldVersion);
            project.Version = newVersion.ToString();
        }
    }
}