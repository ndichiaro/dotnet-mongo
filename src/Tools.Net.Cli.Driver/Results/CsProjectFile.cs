namespace Tools.Net.Cli.Driver.Results
{
    /// <summary>
    /// Respresents a C# project file
    /// </summary>
    public class CsProjectFile
    {
        /// <summary>
        /// The output type of the project
        /// </summary>
        public string OutputType { get; set; }
        
        /// <summary>
        /// The support .NET frameworks
        /// </summary>
        public string TargetFramework { get; set; }

    }
}
