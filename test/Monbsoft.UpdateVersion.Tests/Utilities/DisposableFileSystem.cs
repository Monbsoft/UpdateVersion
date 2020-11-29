using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public class DisposableFileSystem : IDisposable
    {
        private bool _disposed;

        public DisposableFileSystem()
        {
            _disposed = false;
            RootPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(RootPath);
            DirectoryInfo = new DirectoryInfo(RootPath);
        }

        public DirectoryInfo DirectoryInfo { get; set; }
        public string RootPath { get; }

        public DisposableFileSystem CreateFile(string path)
        {
            File.WriteAllText(Path.Combine(RootPath, path), GetDefaultContent());
            return this;
        }
        public DisposableFileSystem CreateFile(string path, string content)
        {
            File.WriteAllText(Path.Combine(RootPath, path), content);
            return this;
        }
        public DisposableFileSystem CreateFolder(string path)
        {
            Directory.CreateDirectory(Path.Combine(RootPath, path));
            return this;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                try
                {
                    Directory.Delete(RootPath, true);
                }
                catch
                {

                }
            }
            _disposed = true;
        }

        private string GetDefaultContent()
        {
            return @"<Project Sdk=""Microsoft.NET.Sdk"">
</Project>";
        }

    }
}
