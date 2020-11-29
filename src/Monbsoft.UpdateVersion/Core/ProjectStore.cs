using Microsoft.Extensions.FileProviders;
using Monbsoft.UpdateVersion.Models;
using Semver;
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Monbsoft.UpdateVersion.Core
{
    public class ProjectStore
    {
        public Project Read(IFileInfo projectFile)
        {
            if (projectFile == null)
            {
                throw new ArgumentNullException(nameof(projectFile));
            }
            var project = new Project(projectFile);

            var projectDocument = ReadProject(projectFile);
            var versionElement = ReadVersionElement(projectDocument);

            if (versionElement != null)
            {
                project.Version = versionElement.Value;
            }

            if (!SemVersion.TryParse(project.Version, out var version))
            {
                version = new SemVersion(0, 1, 0);
                project.Version = version.ToString();
            }
            return project;
        }

        public void Save(Project project)
        {
            var projectDocument = ReadProject(project.File);
            var versionElement = ReadVersionElement(projectDocument);

            SemVersion parsedVersion = SemVersion.Parse(project.Version);

            if (versionElement != null)
            {
                versionElement.Value = parsedVersion.ToString();
            }
            else
            {
                // Find the first non-conditional PropertyGroup
                var propertyGroup = projectDocument.Root.DescendantNodes()
                    .FirstOrDefault(node => node is XElement el
                        && el.Name == "PropertyGroup"
                        && el.Attributes().All(attr =>
                            attr.Name != "Condition")) as XElement;
                // No valid property group, create a new one
                if (propertyGroup == null)
                {
                    propertyGroup = new XElement("PropertyGroup");
                    projectDocument.Root.AddFirst(propertyGroup);
                }
                propertyGroup.Add(new XElement("Version", parsedVersion.ToString()));
            }

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
            };

            using var xw = XmlWriter.Create(project.FullName, settings);
            projectDocument.Save(xw);
        }

        /// <summary>
        /// Reads the xml version element if it exists.
        /// </summary>
        /// <param name="projectDocument"></param>
        /// <returns></returns>
        private XElement ReadVersionElement(XDocument projectDocument)
        {
            return projectDocument.XPathSelectElements("//Version").FirstOrDefault();
        }

        /// <summary>
        /// Reads the project file as XML
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        private XDocument ReadProject(IFileInfo fileInfo)
        {
            return XDocument.Load(fileInfo.PhysicalPath, LoadOptions.PreserveWhitespace);
        }
    }
}