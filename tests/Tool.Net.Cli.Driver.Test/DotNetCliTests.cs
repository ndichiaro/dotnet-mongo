using Tools.Net.Cli.Driver;
using Tools.Net.Cli.Driver.Commands;
using Tools.Net.Cli.Driver.Configuration;
using Xunit;

namespace Tool.Net.Cli.Driver.Test
{
    /// <summary>
    /// A suite of unit tests that validate the functionality
    /// of the DotNetCli class
    /// </summary>
    public class DotNetCliTests
    {
        /// <summary>
        /// Validates the dotnet cli can create a build command
        /// </summary>
        [Fact]
        public void CanCreateBuildCommand()
        {
            var cli = new DotNetCLI("Helpers");

            var result = cli.Build(x => x.BuildConfiguration = BuildConfiguration.Debug);

            Assert.IsType<BuildCommand>(result);
        }
    }
}
