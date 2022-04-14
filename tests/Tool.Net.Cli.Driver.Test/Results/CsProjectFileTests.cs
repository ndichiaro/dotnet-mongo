using System;
using Tools.Net.Cli.Driver.Results;
using Xunit;

namespace Tool.Net.Cli.Driver.Test.Results
{
    public class CsProjectFileTests
    {
        [Theory]
        [InlineData("net6.0", "net6.0")]
        [InlineData("netstandard2.0", "netstandard2.0")]
        [InlineData("netcoreapp3.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;netcoreapp3.1;netstandard2.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;net6.0;netcoreapp3.1;net5.0", "net6.0")]
        public void CanGetLatestTargetFramework_TargetFramework(string targetFramework, string expected)
        {
            var csProjectFile = new CsProjectFile
            {
                TargetFramework = targetFramework
            };

            var actual = csProjectFile.GetLatestTargetFramework();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("net6.0", "net6.0")]
        [InlineData("netstandard2.0", "netstandard2.0")]
        [InlineData("netcoreapp3.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;netcoreapp3.1;netstandard2.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;net6.0;netcoreapp3.1;net5.0", "net6.0")]
        public void CanGetLatestTargetFramework_TargetFrameworks(string targetFrameworks, string expected)
        {
            var csProjectFile = new CsProjectFile
            {
                TargetFrameworks = targetFrameworks
            };

            var actual = csProjectFile.GetLatestTargetFramework();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("net6.0", "net6.0")]
        [InlineData("netstandard2.0", "netstandard2.0")]
        [InlineData("netcoreapp3.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;netcoreapp3.1;netstandard2.1", "netcoreapp3.1")]
        [InlineData("netstandard2.0;net6.0;netcoreapp3.1;net5.0", "net6.0")]
        public void CanGetLatestTargetFramework_Both(string targetFrameworks, string expected)
        {
            var csProjectFile = new CsProjectFile
            {
                TargetFramework = targetFrameworks,
                TargetFrameworks = targetFrameworks
            };

            var actual = csProjectFile.GetLatestTargetFramework();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("invalid")]
        public void CanGetLatestTargetFramework_Invalid(string targetFrameworks)
        {
            var csProjectFile = new CsProjectFile
            {
                TargetFramework = targetFrameworks,
                TargetFrameworks = targetFrameworks
            };

            Assert.Throws<PlatformNotSupportedException>(() => csProjectFile.GetLatestTargetFramework());
        }
    }
}
