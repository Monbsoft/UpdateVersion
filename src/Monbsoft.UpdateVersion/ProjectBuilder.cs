using Monbsoft.UpdateVersion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion
{
    public class ProjectBuilder
    {
        public Project Build(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("No project file found.");
            }
            return new Project
            {
                FullName = path,

            };         
        }
    }
}
