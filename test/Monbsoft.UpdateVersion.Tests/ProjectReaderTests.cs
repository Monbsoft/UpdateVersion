using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System.IO;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class ProjectReaderTests
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

        [Fact]
        public void ReadNoVersionTest()
        {
            string projectFile = "project2.csproj";
            using(var fs  = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, string.Format(ProjectTemplate, string.Empty));
                var reader = new ProjectReader();

                var project = reader.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("0.1.0", project.Version);
            }
        }

        [Fact]
        public void ReadProjectTest()
        {
            string projectFile = "project1.csproj";
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, string.Format(ProjectTemplate, "<Version>0.5</Version>"));
                var reader = new ProjectReader();

                var project = reader.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("0.5", project.Version);
            }
        }

        private static IFileInfo CreateFileInfo(DisposableFileSystem fs, string filename)
        {
            return new PhysicalFileInfo(new FileInfo(Path.Combine(fs.RootPath, filename)));
        }
    }
}