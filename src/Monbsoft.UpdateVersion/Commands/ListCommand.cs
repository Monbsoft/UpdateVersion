using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Monbsoft.UpdateVersion.Commands
{
    public class ListCommand
    {
        public static Command Create()
        {
            var command = new Command("list", "Lists all project versions");

            command.Handler = CommandHandler.Create<ShowCommandArguments>(args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new ListCommand();
                command.Execute(context);
            });

            return command;
        }

        public void Execute(CommandContext context)
        {
            var reader = new ProjectReader();
            var finder = new ProjectFinder(context.Directory);
            var projectFiles = finder.FindProjects();

            foreach (var projectFile in projectFiles)
            {
                var project = reader.Read(projectFile);
                context.WriteInfo($"\t{project.Name} -> {project.Version}");
            }
        }

        private class ShowCommandArguments
        {
            public IConsole Console { get; set; } = default;
            public FileInfo Path { get; set; } = default;
            public Verbosity Verbosity { get; set; } = Verbosity.Info;
        }
    }
}