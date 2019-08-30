using Dot.Net.Cli.Driver.Configuration;

namespace Dot.Net.Cli.Driver.Options
{
    /// <summary>
    /// Options for the dotnet build command
    /// </summary>
    public class BuildCommandOptions
    {
        /// <summary>
        /// Defines the build configuration {Debug|Release}
        /// </summary>
        public BuildConfiguration BuildConfiguration { get; set; }
    }
}
