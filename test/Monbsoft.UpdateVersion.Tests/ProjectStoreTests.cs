using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System;
using System.IO;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class ProjectStoreTests
    {

        [Fact]
        public void ReadNoVersionTest()
        {
            string projectFile = "project2.csproj";
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, ProjectHelper.SetVersion(string.Empty));
                var store = new ProjectStore();

                var project = store.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("0.1.0", project.Version);
            }
        }

        [Fact]
        public void ReadProjectTest()
        {
            string projectFile = "project1.csproj";
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, ProjectHelper.SetVersion("0.5"));
                var store = new ProjectStore();

                var project = store.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("0.5", project.Version);
            }
        }

        [Fact]
        public void ReadProjectWithPatchTest()
        {
            string projectFile = "MyProject.csproj";
            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, ProjectHelper.SetVersion("1.3.5"));
                var store = new ProjectStore();

                var project = store.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("1.3.5", project.Version);
            }
        }

        [Fact]
        public void UpdateProjectTest()
        {
            string projectFile = "MyProject.csproj";

            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, ProjectHelper.SetVersion("1.4.0"));
                var store = new ProjectStore();
                var project = store.Read(CreateFileInfo(fs, projectFile));
                project.Version = "2.0";

                store.Save(project);
                var newProject = store.Read(CreateFileInfo(fs, projectFile));

                Assert.Equal("2.0", project.Version);
            }
        }

        [Fact]
        public void UpdateProjectWithBadVersionTest()
        {
            string projectFile = "MyProject.csproj";

            using (var fs = new DisposableFileSystem())
            {
                fs.CreateFile(projectFile, ProjectHelper.SetVersion("1.4.0"));
                var store = new ProjectStore();
                var project = store.Read(CreateFileInfo(fs, projectFile));
                project.Version = "${BuildVersion}";

                Assert.Throws<ArgumentException>("version", () => store.Save(project));
            }
        }

        private static IFileInfo CreateFileInfo(DisposableFileSystem fs, string filename)
        {
            return new PhysicalFileInfo(new FileInfo(Path.Combine(fs.RootPath, filename)));
        }
    }
}