using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public static class PathHelper
    {
        public static IFileInfo GetFile(DisposableFileSystem fs, string filename)
        {
            var fileInfo = new FileInfo(Path.Combine(fs.RootPath, filename));
            return new PhysicalFileInfo(fileInfo);
        }
    }
}
