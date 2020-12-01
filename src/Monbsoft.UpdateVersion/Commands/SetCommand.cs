using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Monbsoft.UpdateVersion.Commands
{
    public class SetCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = new Command("set", "Sets the version of the projects.");

            var versionArg = new Argument("vesion");
            versionArg.Description = "Version of the projects";
            command.AddArgument(versionArg);

            command.Handler = CommandHandler.Create<SetCommandArguments>(args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new SetCommand();
                command.Execute(context, args.Version);
            });

            return command;
        }

        public void Execute(CommandContext context, string version)
        {
            var newVersion = SemVersion.Parse(version);
            int count = Update(context, (oldVersion) =>
            {
                return newVersion;
            });
            context.WriteInfo($"{count} versions are updated.");
        }

        public class SetCommandArguments
        {
            public IConsole Console { get; set; } = default;
            public Verbosity Verbosity { get; set; } = Verbosity.Info;
            public string Version { get; set; }
        }
    }
}