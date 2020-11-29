using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Monbsoft.UpdateVersion.Models
{
    public class Project
    {
        public Project(IFileInfo file)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));
        }
       
        public IFileInfo File { get; }

        public string FullName => File.PhysicalPath;

        public string Name => File.Name;

        public string Version { get; set; }

        public override string ToString()
        {
            return Name;
        }

    }
}
