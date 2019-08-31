namespace Tools.Net.Cli.Driver.Results
{
    /// <summary>
    /// The results of a command execution
    /// </summary>
    public class CommandResults
    {
        /// <summary>
        /// Indicates if the command was successfully executed
        /// </summary>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// The results of the execution
        /// </summary>
        public string Message { get; set; }
    }
}
