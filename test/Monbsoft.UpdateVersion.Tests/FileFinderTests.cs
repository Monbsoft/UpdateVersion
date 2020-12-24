using Monbsoft.UpdateVersion.Core;
using Monbsoft.UpdateVersion.Tests.Utilities;
using System.IO;
using System.Linq;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests
{
    public class FileFinderTests
    {
        [Fact]
        public void FindWithSolutionFile()
        {
            using(var fs = new DisposableFileSystem())
            {
                fs.CreateFile("MySolution.sln")
                    .CreateFolder("src/project")
                    .CreateFile("src/project/proj1.csproj");
                var finder = new ProjectFinder(fs.RootPath);


                var projectFiles = finder.FindProjects();
                var project = projectFiles.First();


                Assert.Single(projectFiles);
                Assert.Equal("proj1.csproj", project.Name);
            }
        }

        [Fact]
        public void FindWithProjectFiles()
        {
            using(var fs = new DisposableFileSystem())
            {
                fs.CreateFile("project1.csproj")
                    .CreateFile("project2.csproj");
                var finder = new ProjectFinder(fs.RootPath);

                var projects = finder.FindProjects().OrderBy(p => p.Name).ToArray();

                Assert.Equal("project1.csproj", projects[0].Name);
                Assert.Equal("project2.csproj", projects[1].Name);

            }
        }

        [Fact]
        public void FindWithNoProjectOrSolution()
        {
            using(var fs = new DisposableFileSystem())
            {
                fs.CreateFolder("src/project1")
                    .CreateFile("src/project1/project1.csproj");
                var finder = new ProjectFinder(fs.RootPath);

                Assert.Throws<FileNotFoundException>(() => finder.FindProjects());

            }
        }
    }
}
