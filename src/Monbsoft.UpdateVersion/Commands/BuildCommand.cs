﻿using Monbsoft.UpdateVersion.Core;
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
        public BuildCommand(IGitService gitService)
            : base(gitService)
        {

        }

        public static Command Create()
        {
            var command = new Command("build", "Increment build version number");

            command.Handler = CommandHandler.Create<VersionCommandArguments>(async args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory()
                };
                var command = new BuildCommand(new GitService());
                await command.ExecuteAsync(context);
            });

            return command;
        }

        public async Task ExecuteAsync(CommandContext context)
        {
            int count = await UpdateAsync(context, (oldVersion) =>
            {
                if (string.IsNullOrEmpty(oldVersion.Build))
                    throw new ArgumentNullException("Build");

                string[] split = oldVersion.Build.Split('.');
                int last = split.Length - 1;

                if (!int.TryParse(split[last], out int buildVersion))
                {
                    throw new FormatException($"{oldVersion.Build} is not in the correct format.");
                }
                buildVersion++;
                split[last] = buildVersion.ToString();
                return oldVersion.Change(build: string.Join('.', split));
            });
            context.WriteInfo($"{count} build versions are updated.");
        }
    }
}
