using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Core
{
    public interface IGitService
    {
        Task<bool> IsInstalled();
        Task<bool> RunCommandAsync(CommandContext context, string args);
    }
}