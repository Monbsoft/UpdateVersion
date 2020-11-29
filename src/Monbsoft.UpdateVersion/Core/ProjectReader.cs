using Microsoft.Extensions.FileProviders;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Monbsoft.UpdateVersion.Core
{
    public class ProjectReader
    {
        public Project Read(IFileInfo projectFile)
        {
            if(projectFile == null)
            {
                throw new ArgumentNullException(nameof(projectFile));
            }
            var project = new Project(projectFile);

            var projectDocument = XDocument.Load(projectFile.PhysicalPath, LoadOptions.PreserveWhitespace);
            var versionElement = projectDocument.XPathSelectElements("//Version").FirstOrDefault();

            if(versionElement != null)
            {
                project.Version = versionElement.Value;
            }

            if(!SemVersion.TryParse(project.Version, out var version))
            {               
                version = new SemVersion(0, 1, 0);
                project.Version = version.ToString();
            }
            return project;
        }
    }
}
