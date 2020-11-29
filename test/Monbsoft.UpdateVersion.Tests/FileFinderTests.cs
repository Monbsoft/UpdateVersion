using Microsoft.Extensions.FileProviders.Physical;
using Monbsoft.UpdateVersion.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xunit;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public class FileFinderTests
    {
        [Fact]
        public void FindWithSolution()
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
        void FindWithNoProjectOrSolution()
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
