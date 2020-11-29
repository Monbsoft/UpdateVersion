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
using Xunit.Abstractions;

namespace Monbsoft.UpdateVersion.Tests
{
    public class MajorTests
    {
        private const string ProjectTemplate = @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFrameworks>net5.0</TargetFrameworks>
    {0}
  </PropertyGroup>
  <ItemGroup>
    <Compile Include=""**\*.cs"" Exclude=""Excluded.cs;$(DefaultItemExcludes)"" />
  </ItemGroup>
</Project>";
        private TestConsole _console;

        public MajorTests()
        {
            _console = new TestConsole();
        }

        [Fact]
        public void ChangeMajorVersionTest()
        {
            using(var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln");
                fs.CreateFolder("src/Services");
                fs.CreateFile("src/Services/project1.csproj", ProjectHelper.BuildVersion("1.5.1"));
                var store = new ProjectStore();
                var command = new MajorCommand();
                var context = new CommandContext(_console, Verbosity.Info);
                context.Directory = fs.RootPath;

                command.Execute(context);
                var project  = store.Read(PathHelper.GetFile(fs, "src/Services/project1.csproj"));

                Assert.Equal("2.0.0", project.Version);

            }
        }
    }
}
