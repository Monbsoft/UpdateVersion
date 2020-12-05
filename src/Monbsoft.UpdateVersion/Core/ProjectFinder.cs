using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Monbsoft.UpdateVersion.Core
{
    public class ProjectFinder
    {
        private static readonly string ProjectFormat = "*.csproj";
        private static readonly string SolutionFormat = "*.sln";
        private readonly DirectoryInfo _currentFolder;

        public ProjectFinder(string folderPath)
        {
            _currentFolder = new DirectoryInfo(folderPath);
            if(!_currentFolder.Exists)
                throw new DirectoryNotFoundException(_currentFolder.Name);
        }

        public List<IFileInfo> FindProjects()
        {
            List<IFileInfo> projectFiles = new List<IFileInfo>();
            // try project files in current folder
            if (TryProjectFiles(true, out projectFiles))
            {
                return projectFiles;
            }
            // try
            if (ExistsSolutionFile())
            {
                TryProjectFiles(false, out projectFiles);
            }
            else
            {
                throw new FileNotFoundException("No project or solution file is found.");
            }

            return projectFiles;
        }
        private bool ExistsSolutionFile()
        {
            var files = _currentFolder.GetFiles(SolutionFormat);

            if (files.Length > 1)
                throw new FileNotFoundException("More solution files are found.");
            
            if (files.Length == 0)
                return false;
            
            return true;
        }

        private bool TryProjectFiles(bool inCurrentFolder, out List<IFileInfo> projectFiles)
        {
            var files = _currentFolder.GetFiles();
            SearchOption searchOption = SearchOption.AllDirectories;

            if(inCurrentFolder)
            {
                searchOption = SearchOption.TopDirectoryOnly;
            }

            if (!files.Any())
            {
                projectFiles = default;
                return false;
            }
            projectFiles = _currentFolder.EnumerateFiles(ProjectFormat, searchOption)
                .Select(file => new PhysicalFileInfo(file)).ToList<IFileInfo>();
            return projectFiles.Any();
        }
    }
}