using System;
using Tools.Net.Cli.Driver.Configuration;

namespace Tools.Net.Cli.Driver.Commands
{
    /// <summary>
    /// The dotnet build command
    /// </summary>
    public class BuildCommand : Command
    {
        #region Variables
        private readonly BuildConfiguration _configuration;
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a BuildCommand instance
        /// </summary>
        /// <param name="configuration">Defines the build configuration</param>
        /// <param name="workingDirectory">The directory to run the command</param>
        /// <param name="baseCommand">The command to run before `build`</param>
        public BuildCommand(BuildConfiguration configuration, string workingDirectory, string baseCommand)
            : base(workingDirectory, baseCommand)
        {
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Creates the dotnet command runner
        /// </summary>
        /// <exception cref="ArgumentNullException" />
        public override CommandRunner Create()
        {
            if (string.IsNullOrEmpty(BaseCommand)) throw new ArgumentNullException("A base command must be provided to use the build command");
            return new CommandRunner(WorkingDirectory, $"{BaseCommand} build");
        }
        #endregion
    }
}
