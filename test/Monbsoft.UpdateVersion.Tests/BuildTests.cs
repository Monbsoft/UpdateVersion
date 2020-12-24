using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class BuildTests
    {
        private TestConsole _console;

        public BuildTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public async Task ChangeBuildVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.15.0-alpha+9"));
                var store = new ProjectStore();
                var command = new BuildCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("1.15.0-alpha+10", project.Version);

            }
        }

        [Fact]
        public async Task ChangeBuildWithBadFormatTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("5.0.8+15f"));
                var store = new ProjectStore();
                var command = new BuildCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await Assert.ThrowsAsync<FormatException>(() => command.ExecuteAsync(context));
            }
        }
        [Fact]
        public async Task ChangeWithComplexBuildVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.15.0-alpha+2020.119"));
                var store = new ProjectStore();
                var command = new BuildCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await command.ExecuteAsync(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("1.15.0-alpha+2020.120", project.Version);

            }
        }
        [Fact]
        public async Task ChangeWithNoBuildVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("5.0.8"));
                var store = new ProjectStore();
                var command = new BuildCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                await Assert.ThrowsAsync<ArgumentNullException>(() => command.ExecuteAsync(context));
            }
        }
    }
}
