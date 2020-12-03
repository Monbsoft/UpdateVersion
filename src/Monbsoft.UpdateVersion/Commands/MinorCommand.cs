using Monbsoft.UpdateVersion.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class MinorCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = new Command("minor", "Increments minor version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(async args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new MinorCommand();
                await command.ExecuteAsync(context);
            });

            return command;
        }

        public async Task ExecuteAsync(CommandContext context)
        {
            int count = await UpdateAsync(context, (oldVersion) =>
            {
                return oldVersion.Change(minor: oldVersion.Minor + 1, patch: 0);
            });
            context.WriteInfo($"{count} minor versions are updated.");
        }
    }
}