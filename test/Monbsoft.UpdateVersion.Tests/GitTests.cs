using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class GitTests
    {
        private TestConsole _console;

        public GitTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public async Task ChangeVersionWithoutNoGitToolTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var command = new MajorCommand(GitHelper.CreateDefaultGitMock().Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;
                context.Message = "test";

                var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync(context));

                Assert.Equal("Unable to commit because git is not installed.", exception.Message);
            }
        }

        [Fact]
        public async Task AddTagTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var gitMock = GitHelper.CreateGitMock(true);

                var command = new MajorCommand(gitMock.Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;
                context.Tag = true;

                await command.ExecuteAsync(context);

                gitMock.Verify(git => git.IsInstalled());
                gitMock.Verify(git => git.RunCommandAsync(It.IsAny<CommandContext>(), It.Is<string>(args => args.Equals("tag -a v2.0.0 -m \"Version 2.0.0\""))));
            }
        }

        [Fact]
        public async Task CreateCommitTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var gitMock = GitHelper.CreateGitMock(true);
               
                var command = new MajorCommand(gitMock.Object);
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;
                context.Message = "test";

                await command.ExecuteAsync(context);

                gitMock.Verify(git => git.IsInstalled());
                gitMock.Verify(git => git.RunCommandAsync(It.IsAny<CommandContext>(), It.Is<string>(args => args.Equals("commit -a -m \"test\""))));
            }
        }
    }
}
