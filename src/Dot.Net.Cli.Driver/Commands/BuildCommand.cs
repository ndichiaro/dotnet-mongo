using DotNet.Cli.Driver.Configuration;
using System;

namespace DotNet.Cli.Driver.Commands
{
    /// <summary>
    /// The dotnet build command
    /// </summary>
    public class BuildCommand : Command
    {
        private readonly BuildConfiguration _configuration;

        public BuildCommand(BuildConfiguration configuration, string workingDirectory, string baseCommand) 
            : base(workingDirectory, baseCommand)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates the dotnet build command
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public override CommandRunner Create()
        {
            if (string.IsNullOrEmpty(BaseCommand)) throw new ArgumentNullException("A base command must be provided to use the build command");
            return new CommandRunner(WorkingDirectory, $"{BaseCommand} build");
        }
    }
}
