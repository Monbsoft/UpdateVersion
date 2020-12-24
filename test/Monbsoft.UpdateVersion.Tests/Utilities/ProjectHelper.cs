using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monbsoft.UpdateVersion.Tests.Utilities
{
    public static class ProjectHelper
    {
        private const string ProjectTemplate = @"<Project Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFrameworks>net5.0</TargetFrameworks>
    {0}
  </PropertyGroup>
  <ItemGroup>
    <Compile Include=""**\*.cs"" Exclude=""Excluded.cs;$(DefaultItemExcludes)"" />
  </ItemGroup>
</Project>";

        public static string SetVersion(string version)
        {
            return string.Format(ProjectTemplate, $"<Version>{version}</Version>");
        }
    }
}
