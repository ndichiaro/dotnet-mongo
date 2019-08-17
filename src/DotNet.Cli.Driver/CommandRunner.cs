using DotNet.Cli.Driver.Results;
using DotNet.Cli.Driver.Tools;

namespace DotNet.Cli.Driver
{
    /// <summary>
    /// Runs CLI commands
    /// </summary>
    public class CommandRunner
    {
        private readonly string _workingDirectory;
        private readonly string _command;

        /// <summary>
        /// Creates a CommandRunner instance
        /// </summary>
        /// <param name="workingDirectory">the working directory for the command</param>
        /// <param name="command">the command to be run</param>
        public CommandRunner(string workingDirectory, string command)
        {
            _workingDirectory = workingDirectory;
            _command = command;
        }

        /// <summary>
        /// Runs the command
        /// </summary>
        /// <returns>the results of the command</returns>
        public CommandResults Run()
        {
            var results = new CommandResults();

            var terminalResults = Terminal.Execute(_workingDirectory, _command);

            return results;
        }
    }
}
