using Monbsoft.UpdateVersion.Models;
using System;
using System.CommandLine;
using System.CommandLine.IO;

namespace Monbsoft.UpdateVersion.Core
{
    public class CommandContext
    {
        public CommandContext(IConsole console, Verbosity verbosity)
        {
            if (console == null)
            {
                throw new ArgumentNullException(nameof(console));
            }
            Console = console;
            Verbosity = verbosity;
        }

        public IConsole Console { get; set; }
        public string Directory { get; set; }
        public string Message { get; set; }
        public Verbosity Verbosity { get; set; }

        public void WriteLine(Verbosity verbosity, string message)
        {
            if (Verbosity >= verbosity)
            {
                Console.Out.WriteLine(message);
            }
        }

        public void WriteDebug(string message)
        {
            WriteLine(Verbosity.Debug, message);
        }

        public void WriteInfo(string message)
        {
            WriteLine(Verbosity.Info, message);
        }
    }
}