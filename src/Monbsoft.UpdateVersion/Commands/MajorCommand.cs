using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
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
    public class MajorCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = new Command("major", "Increment major version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new MajorCommand();
                command.Execute(context);
            });

            return command;
        }

        public void Execute(CommandContext context)
        {
            int count = Update(context, (oldVersion) =>
            {
                return oldVersion.Change(major: oldVersion.Major + 1, 0, 0);
            });
            context.WriteInfo($"{count} major versions are updated.");
        }
    }
}
