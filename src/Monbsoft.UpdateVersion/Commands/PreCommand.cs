using Monbsoft.UpdateVersion.Core;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class PreCommand : VersionCommandBase
    {
        public PreCommand(IGitService gitService)
            : base(gitService)
        {

        }

        public static Command Create()
        {
            var command = CreateCommand("pre", "Increment pre-release version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(async args =>
            {
                var command = new PreCommand(new GitService());
                await command.ExecuteAsync(CreateCommandContext(args));
            });

            return command;
        }

        public async Task ExecuteAsync(CommandContext context)
        {
            context.WriteDebug("Updating pre-release versions...");
            int count = await UpdateAsync(context, (oldVersion) =>
            {
                if (string.IsNullOrEmpty(oldVersion.Prerelease))
                    throw new ArgumentNullException("Pre-release");

                string[] split = oldVersion.Prerelease.Split('.');
                int last = split.Length - 1;

                if (!int.TryParse(split[last], out int preVersion))
                {
                    throw new FormatException($"{oldVersion.Prerelease} is not in the pre-release correct format.");
                }
                preVersion++;
                split[last] = preVersion.ToString();
                return oldVersion.Change(prerelease: string.Join('.', split));
            });
            context.WriteInfo($"{count} pre-release versions updated.");
        }
    }
}
