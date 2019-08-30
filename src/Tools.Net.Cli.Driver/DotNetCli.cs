using System;
using Tools.Net.Cli.Driver.Commands;
using Tools.Net.Cli.Driver.Options;

namespace Tools.Net.Cli.Driver
{
    /// <summary>
    /// Represents the dotnet cli
    /// </summary>
    public class DotNetCLI
    {
        /// <summary>
        /// The base command for the dotnet cli
        /// </summary>
        private const string Command = "dotnet";

        /// <summary>
        /// The working directory for the cli
        /// </summary>
        private readonly string _workingDirectory;

        /// <summary>
        /// Creates a dotnet cli object
        /// </summary>
        /// <param name="workingDirectory">The working directory for the cli</param>
        public DotNetCLI(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        /// <summary>
        /// Creates the dotnet build command
        /// </summary>
        /// <param name="configuration">Defines the build configuration {Debug|Release}</param>
        /// <returns>a dotnet build command</returns>
        public BuildCommand Build(Func<object, BuildCommandOptions> options)
        {
            var commandOptions = options(new BuildCommandOptions());

            return new BuildCommand(commandOptions.BuildConfiguration, _workingDirectory, Command);
        }
    }
}
