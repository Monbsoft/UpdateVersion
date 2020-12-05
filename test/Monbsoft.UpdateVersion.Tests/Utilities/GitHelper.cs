using Monbsoft.UpdateVersion.Core;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public static class GitHelper
    {
        public static Mock<IGitService> CreateDefaultGitMock() => CreateGitMock(false);

        public static Mock<IGitService> CreateGitMock(bool installed)
        {
            var mock = new Mock<IGitService>();
            mock.Setup(git => git.IsInstalled())
                .Returns(Task.FromResult(installed));
            mock.Setup(git => git.RunCommandAsync(It.IsAny<CommandContext>(), It.IsAny<string>()))
                .Returns(Task.FromResult(true));

            return mock;
        }
    }
}
