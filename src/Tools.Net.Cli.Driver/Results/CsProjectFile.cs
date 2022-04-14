using System;
using System.Linq;

namespace Tools.Net.Cli.Driver.Results
{
    /// <summary>
    /// Respresents a C# project file
    /// </summary>
    public class CsProjectFile
    {
        /// <summary>
        /// The support .NET framework(s)
        /// </summary>
        public string TargetFramework { get; set; }

        /// <summary>
        /// The support .NET framework(s)
        /// </summary>
        public string TargetFrameworks { get; set; }

        public string GetLatestTargetFramework()
        {
            var targetFramework = TargetFramework ?? TargetFrameworks;

            if (targetFramework != null)
            {
                var targetFrameworks = targetFramework.Split(';');
                // well check for net, netcoreapp, then netstandard based on more recent versions
                Func<string, bool> hasNetSupport = x => x.Contains("net") && !x.Contains("netcoreapp") && !x.Contains("netstandard");
                if (targetFrameworks.Any(hasNetSupport))
                {
                    return targetFrameworks.Where(hasNetSupport).OrderBy(x => x).Last();
                }

                Func<string, bool> hasNetCoreSupport = x => x.Contains("netcoreapp");
                if (targetFrameworks.Any(hasNetCoreSupport))
                {
                    return targetFrameworks.Where(hasNetCoreSupport).OrderBy(x => x).Last();
                }

                Func<string, bool> hasNetStandardSupport = x => x.Contains("netstandard");
                if (targetFrameworks.Any(hasNetStandardSupport))
                {
                    return targetFrameworks.Where(hasNetStandardSupport).OrderBy(x => x).Last();
                }
            }
            throw new PlatformNotSupportedException();
        }

    }
}
