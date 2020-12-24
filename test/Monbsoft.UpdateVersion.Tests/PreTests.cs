using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class PreTests
    {
        private TestConsole _console;

        public PreTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public async Task ChangePreVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.SetVersion("3.1.5-10"));
                var store = new ProjectStore();
                var command = new PreCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("3.1.5-11", project.Version);

            }
        }

        [Fact]
        public async Task ChangePreVersionWithBuildNumberTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.SetVersion("3.1.5-10+2020"));
                var store = new ProjectStore();
                var command = new PreCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("3.1.5-11+2020", project.Version);

            }
        }

        [Fact]
        public async Task ChangePreWithBadFormatTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.SetVersion("5.0.8-beta"));
                var store = new ProjectStore();
                var command = new PreCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await Assert.ThrowsAsync<FormatException>(() => command.ExecuteAsync(context));
            }
        }

        [Fact]
        public async Task ChangePreWithComplexPreTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.SetVersion("1.15.0-beta.119"));
                var store = new ProjectStore();
                var command = new PreCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("1.15.0-beta.120", project.Version);

            }
        }
    }
}
