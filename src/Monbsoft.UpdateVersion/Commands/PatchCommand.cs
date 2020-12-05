using Monbsoft.UpdateVersion.Core;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class PatchCommand : VersionCommandBase
    {
        public PatchCommand(IGitService gitService)
            : base(gitService)
        {

        }
        public static Command Create()
        {
            var command = CreateCommand("patch", "Increment patch version number");
            
            command.Handler = CommandHandler.Create<VersionCommandArguments>(async args =>
            {
                var command = new PatchCommand(new GitService());
                await command.ExecuteAsync(CreateCommandContext(args));
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
