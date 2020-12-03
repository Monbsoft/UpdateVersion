using System;
using System.Collections.Generic;
using System.CommandLine.Invocation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Core
{
    public static class GitUtils
    {
        public static async Task<bool> IsInstalled(string workingDir)
        {
            try
            {
                var result = await Process.ExecuteAsync("git", "--version", workingDir: workingDir);
                return result == 0;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
