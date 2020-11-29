using Microsoft.Extensions.FileProviders.Physical;
using Monbsoft.UpdateVersion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public class ProjectBuilder
    {
        private readonly DisposableFileSystem _fs;

        public ProjectBuilder(DisposableFileSystem fs)
        {
            _fs = fs;
        }

        public Project Build(string name, string version)
        {
            return BuildProject(name, BuildContent(version));
        }

        public Project BuildNoVersion(string name)
        {
            var content = @$"<Project Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <Authors></Authors>
        <AssemblyName></AssemblyName>
    </PropertyGroup>
</Project>";
            return BuildProject(name, content);
        }

        private Project BuildProject(string name, string content)
        {
            string filepath = Path.Combine(_fs.RootPath, name);
            _fs.CreateFile(filepath, content);

            var projectFile = new PhysicalFileInfo(new FileInfo(filepath));
            return new Project(projectFile);
        }

        private string BuildContent(string version)
        {
            return @$"<Project Sdk=""Microsoft.NET.Sdk"">
    <PropertyGroup>
        <OutputType>Exe</OutputType>
        <TargetFramework>netcoreapp3.1</TargetFramework>
        <Version>{version}</Version>
        <Authors> </Authors>
        <AssemblyName></AssemblyName >
    </PropertyGroup>
</Project>";
        }

    }
}
