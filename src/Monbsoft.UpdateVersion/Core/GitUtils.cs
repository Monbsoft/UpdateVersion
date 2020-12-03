using System;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Core
{
    public static class GitUtils
    {
        /// <summary>
        /// Determines if git is installed.
        /// </summary>
        /// <returns></returns>
        public static async Task<bool> IsInstalled()
        {
            try
            {
                var result = await Process.ExecuteAsync("git", "--version");
                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Runs the git command.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<bool> RunCommandAsync(string args)
        {
            if (string.IsNullOrEmpty(args))
                throw new ArgumentNullException(nameof(args));

            try
            {
                var result = await Process.ExecuteAsync("git", args);
                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}