using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monbsoft.UpdateVersion.Core
{
    public class FileFinder
    {
        private static readonly string ProjectFormat = "*.csproj";
        private static readonly string SolutionFormat = "*.sln";

        public List<string> FindProjects(string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException("No project or solution folder is found.");
            }

            List<string> projectFiles = new List<string>();
            if (!TryProjectFiles(directoryPath, out projectFiles) && ExistsSolutionFile(directoryPath))
            {
                projectFiles = ResolveProjectFiles(directoryPath);
            }
            else
            {
                throw new FileNotFoundException("No project or solution file is found.");
            }

            return projectFiles;
        }
        private bool ExistsSolutionFile(string directoryPath)
        {
            var files = Directory.GetFiles(directoryPath, SolutionFormat);

            if (files.Length > 1)
            {
                throw new FileNotFoundException("More solution files are found.");
            }
            if (files.Length == 0)
            {
                return false;
            }

            return true;
        }
        private List<string> ResolveProjectFiles(string directoryPath)
        {
            return Directory.EnumerateFiles(directoryPath, ProjectFormat, SearchOption.AllDirectories).ToList();
        }

        private bool TryProjectFiles(string directoryPath, out List<string> projectFilePaths)
        {
            var files = Directory.GetFiles(directoryPath, ProjectFormat);

            if (!files.Any())
            {
                projectFilePaths = default;
                return false;
            }
            projectFilePaths = files.ToList();
            return true;
        }
    }
}