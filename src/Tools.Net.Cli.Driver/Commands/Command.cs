namespace Tools.Net.Cli.Driver.Commands
{
    /// <summary>
    /// Represent a CLI command
    /// </summary>
    public abstract class Command
    {
        #region Variables
        /// <summary>
        /// The directory for the command to run
        /// </summary>
        protected string WorkingDirectory { get; set; }

        /// <summary>
        /// A command to be appended to the command, e.g. In the case of 'dotnet build'
        /// dotnet is the base command and build is the command.
        /// </summary>
        protected string BaseCommand { get; set; }
        #endregion

        /// <summary>
        /// Creates a Command instance
        /// </summary>
        /// <param name="workingDirectory">The working directory of the command</param>
        /// <param name="baseCommand">A base command to be executed with the command</param>
        public Command(string workingDirectory, string baseCommand)
        {
            WorkingDirectory = workingDirectory;
            BaseCommand = baseCommand;
        }

        /// <summary>
        /// creates a command runner
        /// </summary>
        public abstract CommandRunner Create();
    }
}
