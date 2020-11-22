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
    public class ListCommand
    {
        public static Command Create()
        {
            var command = new Command("list", "Lists all project versions");


            command.Handler = CommandHandler.Create<ShowCommandArguments>(args =>
            {
                var context = new CommandContext
                {
                    Directory = Directory.GetCurrentDirectory(),
                    Console = args.Console
                };
                var command = new ListCommand();
                command.Execute(context);

                
            });

            return command;
        }

        public void Execute(CommandContext context)
        {
            var finder = new FileFinder();
            var projectFiles = finder.FindProjects(context.Directory);

            foreach(var projectFile in projectFiles)
            {
                context.Console.Out.Wire
            }
            
            
        }

        private class ShowCommandArguments
        {
            public IConsole Console { get; set; } = default;
            public FileInfo Path { get; set; } = default;

            public Verbosity MyProperty { get; set; }
        }
    }


}
