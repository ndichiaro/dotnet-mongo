using DotNet.Cli.Driver.Commands;
using DotNet.Cli.Driver.Types;

namespace DotNet.Cli.Driver
{
    /// <summary>
    /// Represents the dotnet cli
    /// </summary>
    public class DotNetCLI
    {
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
        /// <returns></returns>
        public BuildCommand Build(BuildConfiguration configuration)
        {
            return new BuildCommand(configuration)
            {
                WorkingDirectory = _workingDirectory
            };
        }
    }
}
