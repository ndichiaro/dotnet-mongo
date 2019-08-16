using DotNet.Cli.Driver.Commands;

namespace DotNet.Cli.Driver
{
    /// <summary>
    /// Represents a command line interface 
    /// </summary>
    public static class CLI
    {
        /// <summary>
        /// Creates the dotnet cli tool
        /// </summary>
        /// <param name="workingDirectory">The working directory for the cli</param>
        /// <returns></returns>
        public static DotNetCLI DotNet(string workingDirectory)
        {
            return new DotNetCLI(workingDirectory);
        }
    }
}
