using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System.Threading.Tasks;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class MinorTests
    {
        private TestConsole _console;

        public MinorTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public async Task ChangeMinorVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var command = new MinorCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("1.6.0", project.Version);

            }
        }
    }
}
