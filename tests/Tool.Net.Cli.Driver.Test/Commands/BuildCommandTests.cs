using System;
using Tools.Net.Cli.Driver;
using Tools.Net.Cli.Driver.Commands;
using Tools.Net.Cli.Driver.Configuration;
using Xunit;

namespace Tool.Net.Cli.Driver.Test.Commands
{
    /// <summary>
    /// A suite of unit tests the validate the functionality of the 
    /// BuildCommand class
    /// </summary>
    public class BuildCommandTests
    {
        /// <summary>
        /// Tests that when the BuildCommand object is created with a null or empty base
        /// command, that an ArguementNullException is thrown
        /// </summary>
        /// <param name="baseCommand">Test data</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void HandlesNullOrEmptyBaseCommand(string baseCommand)
        {
            var buildCommand = new BuildCommand(BuildConfiguration.Release, @"C:\Tests", baseCommand);

            Assert.Throws<ArgumentNullException>(() => buildCommand.Create());
        }

        /// <summary>
        /// Test that a CommandRunner can successfully be created
        /// </summary>
        [Fact]
        public void CanCreateBuildRunner()
        {
            var buildCommand = new BuildCommand(BuildConfiguration.Release, @"C:\Tests", "dotnet");

            var result = buildCommand.Create();

            Assert.IsType<CommandRunner>(result);
        }
    }
}
