using Monbsoft.UpdateVersion.Commands;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Models;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class SetTests
    {
        private TestConsole _console;

        public SetTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public void ChangeVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var command = new SetCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                command.Execute(context, "3.0.1");
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("3.0.1", project.Version);

            }
        }

        [Fact]
        public void ChangeVersionsTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln")
                    .CreateFolder("src/Services/project1")
                    .CreateFile("src/Services/project1/project1.csproj", ProjectHelper.BuildVersion("1.5.1"))
                    .CreateFolder("src/Services/project2")
                    .CreateFile("src/Services/project2/project2.csproj", ProjectHelper.BuildVersion("2.1.0"));
                var store = new ProjectStore();
                var command = new SetCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                command.Execute(context, "4.0.12");
                var project1 = store.Read(PathHelper.GetFile(fs, "src/Services/project1/project1.csproj"));
                var project2 = store.Read(PathHelper.GetFile(fs, "src/Services/project2/project2.csproj"));

                Assert.Equal("4.0.12", project1.Version);
                Assert.Equal("4.0.12", project2.Version);

            }
        }

        [Fact]
        public void ChangeNullVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln")
                    .CreateFolder("src/Services/project1")
                    .CreateFile("src/Services/project1/project1.csproj", ProjectHelper.BuildVersion("1.5.1"))
                    .CreateFolder("src/Services/project2")
                    .CreateFile("src/Services/project2/project2.csproj", ProjectHelper.BuildVersion("2.1.0"));
                var store = new ProjectStore();
                var command = new SetCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                var exception = Assert.Throws<ArgumentNullException>(() => command.Execute(context, null));

                Assert.Equal("version", exception.ParamName);

            }
        }
    }
}
