using Tools.Net.Cli.Driver;
using Xunit;

namespace Tool.Net.Cli.Driver.Test
{
    /// <summary>
    /// A suite of unit tests that validate the functionality 
    /// of the CLI class
    /// </summary>
    public class CLITests
    {
        /// <summary>
        /// Tests that an instance of the DotNetCli class 
        /// can be created
        /// </summary>
        [Fact]
        public void CanCreateDotNetCli()
        {
            var result = CLI.DotNet(x => x.WorkingDirectory = "Helpers");

            Assert.IsType<DotNetCLI>(result);
        }
    }
}
