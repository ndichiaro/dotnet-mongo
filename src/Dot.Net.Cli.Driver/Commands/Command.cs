namespace DotNet.Cli.Driver.Commands
{
    /// <summary>
    /// Represent a CLI command
    /// </summary>
    public abstract class Command
    {
        protected string WorkingDirectory { get; set; }
        protected string BaseCommand { get; set; }

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
        /// creates the command
        /// </summary>
        public abstract CommandRunner Create();
    }
}
