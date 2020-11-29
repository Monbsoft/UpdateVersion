using Microsoft.Extensions.FileProviders.Physical;
using Monbsoft.UpdateVersion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Core
{
    public class ProjectFactory
    {
        public Project Create(string filePath)
        {
            return new Project(new PhysicalFileInfo(new FileInfo(filePath)));
        }
    }
}
