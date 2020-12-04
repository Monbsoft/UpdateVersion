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
        /// Runs git command
        /// </summary>
        /// <param name="context"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<bool> RunCommandAsync(CommandContext context, string args)
        {
            context.WriteDebug($"Running git {args}...");

            if (string.IsNullOrEmpty(args))
                throw new ArgumentNullException(nameof(args));

            try
            {
                var result = await Process.ExecuteAsync("git", args, workingDir: context.Directory);
                return result == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}