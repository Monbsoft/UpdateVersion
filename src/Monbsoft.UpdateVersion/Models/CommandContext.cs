using System.CommandLine;

namespace Monbsoft.UpdateVersion
{
    public class CommandContext
    {
        public IConsole Console { get; set; }
        public string Directory { get; set; }
    }
}
