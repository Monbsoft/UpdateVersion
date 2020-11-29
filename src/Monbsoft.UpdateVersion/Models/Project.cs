using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Models
{
    public class Project
    {
        public Project(IFileInfo file)
        {
            File = file;
        }
       
        public IFileInfo File { get; }

        public string FullName => File.PhysicalPath;

        public string Name => File.Name;

        public string Version { get; set; }
    }
}
