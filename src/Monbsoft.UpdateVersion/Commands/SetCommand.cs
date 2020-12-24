using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Commands
{
    public class SetCommand : VersionCommandBase
    {
        public SetCommand(IGitService gitService)
            : base(gitService)
        {
        }

        public static Command Create()
        {
            var command = CreateCommand("set", "Set the version of the projects.");

            var versionArg = new Argument("version");
            versionArg.Description = "Version of the projects";
            command.AddArgument(versionArg);

            command.Handler = CommandHandler.Create<SetCommandArguments>(async args =>
            {
                var context = new CommandContext(args.Console, args.Verbosity)
                {
                    Directory = Directory.GetCurrentDirectory(),
                    Message = args.Message,
                    Verbosity = args.Verbosity
                };
                var command = new SetCommand(new GitService());
                await command.ExecuteAsync(context, args.Version);
            });

            return command;
        }

        /// <summary>
        /// Executes the set command
        /// </summary>
        /// <param name="context"></param>
        /// <param name="version"></param>
        public async Task ExecuteAsync(CommandContext context, string version)
        {
            context.WriteDebug("Setting versions.");

            if (string.IsNullOrEmpty(version))
                throw new ArgumentNullException(nameof(version));

            var newVersion = SemVersion.Parse(version);
            int count = await UpdateAsync(context, (oldVersion) =>
            {
                return newVersion;
            });
            context.WriteInfo($"{count} versions set.");
        }

        public class SetCommandArguments
        {
            public IConsole Console { get; set; } = default;
            public string Message { get; set; }
            public bool Tag { get; set; }
            public Verbosity Verbosity { get; set; } = Verbosity.Info;
            public string Version { get; set; }
        }
    }
}