using Monbsoft.UpdateVersion.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class PatchCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = CreateCommand("patch", "Increment patch version number");
            
            command.Handler = CommandHandler.Create<VersionCommandArguments>(async args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory(),
                    Message = args.Message,
                    Verbosity = args.Verbosity
                };
                var command = new PatchCommand();
                await command.ExecuteAsync(context);
            });

            return command;
        }

        public async Task ExecuteAsync(CommandContext context)
        {
            int count = await UpdateAsync(context, (oldVersion) =>
            {
                return oldVersion.Change(patch: oldVersion.Patch + 1);
            });
            context.WriteInfo($"{count} patch versions are updated.");
        }
    }
}
