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
    public class BuildCommand : VersionCommandBase
    {
        public static Command Create()
        {
            var command = new Command("build", "Increment build version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new PatchCommand();
                command.Execute(context);
            });

            return command;
        }

        public void Execute(CommandContext context)
        {
            int count = Update(context, (oldVersion) =>
            {
                string build = "";
                if(int.TryParse(oldVersion.Build, out int buildNumber))
                {
                    build = (buildNumber + 1).ToString();
                }
                return oldVersion.Change(build: oldVersion.Build + 1);
            });
            context.WriteInfo($"{count} build versions are updated.");
        }
    }
}
