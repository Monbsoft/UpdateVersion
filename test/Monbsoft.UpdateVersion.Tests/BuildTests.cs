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
        public void ChangeBuildVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("3.63.4"));
                var store = new ProjectStore();
                var command = new BuildCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                command.Execute(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("3.63.4+1", project.Version);

            }
        }

        [Fact]
        public void ChangeExistingBuildVersionTest()
        {
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("5.0.8+14.154"));
                var store = new ProjectStore();
                var command = new BuildCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                command.Execute(context);
                var project = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("5.0.8+15", project.Version);

            }
        }
    }
}
