using Monbsoft.UpdateVersion.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace Monbsoft.UpdateVersion.Commands
{
    public class MinorCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = new Command("minor", "Increments minor version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new MinorCommand();
                command.Execute(context);
            });

            return command;
        }

        public void Execute(CommandContext context)
        {
            int count = Update(context, (oldVersion) =>
            {
                return oldVersion.Change(minor: oldVersion.Minor + 1, patch: 0);
            });
            context.WriteInfo($"{count} minor versions are updated.");
        }
    }
}