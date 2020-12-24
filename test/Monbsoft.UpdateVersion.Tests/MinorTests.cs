using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class PatchTests
    {
        private TestConsole _console;

        public PatchTests()
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
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.SetVersion("1.5.1"));
                var store = new ProjectStore();
                var gitMock = new Mock<IGitService>();
                var command = new MinorCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("1.6.0", project.Version);

            }
        }
    }
}
