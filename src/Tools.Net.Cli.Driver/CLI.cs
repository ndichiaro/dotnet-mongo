using System;
using Tools.Net.Cli.Driver.Options;

namespace Tools.Net.Cli.Driver
{
    /// <summary>
    /// Represents a command line interface 
    /// </summary>
    public static class CLI
    {
        /// <summary>
        /// Creates the dotnet cli tool
        /// </summary>
        /// <param name="options">dotnet cli options</param>
        /// <returns>a dotnet cli instance</returns>
        public static DotNetCLI DotNet(Func<DotNetCliOptions, string> options)
        {
            var workingDirectory = options(new DotNetCliOptions());
            return new DotNetCLI(workingDirectory);
        }
    }
}
