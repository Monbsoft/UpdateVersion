using Monbsoft.UpdateVersion.Commands;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.CommandLine.Parsing;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion
{
    class Program
    {       
        public static Task<int> Main(string[] args)
        {
            var command = new RootCommand()
            {
                Description = "Developper tool to update the Visual Studio project versions."
            };
            command.AddCommand(ListCommand.Create());
            command.AddCommand(MajorCommand.Create());
            command.AddCommand(MinorCommand.Create());
            command.AddCommand(PatchCommand.Create());

            var builder = new CommandLineBuilder(command)
                .UseHelp()
                .UseDefaults()
                .UseVersionOption()
                .CancelOnProcessTermination()
                .UseExceptionHandler();

            var parser = builder.Build();
            return parser.InvokeAsync(args);
        }

        private static void HandleException(Exception ex, InvocationContext context)
        {
            context.Console.Error.WriteLine();
            context.Console.Error.WriteLine(ex.Message);

            context.ResultCode = 1;
        }
    }
}
