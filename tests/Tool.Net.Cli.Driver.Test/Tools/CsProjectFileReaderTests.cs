using System.IO;
using Tools.Net.Cli.Driver.Tools;
using Xunit;

namespace Tool.Net.Cli.Driver.Test.Tools
{
    /// <summary>
    /// A suite of unit tests the validate the functionality of the 
    /// CsProjectFileReader class
    /// </summary>
    public class CsProjectFileReaderTests
    {
        /// <summary>
        /// Tests that if no file is found that a FileNotFoundException
        /// is thrown.
        /// </summary>
        [Fact]
        public void HandleNoProjectFile()
        {
            const string filePath = @"test.xml";

            Assert.Throws<FileNotFoundException>(() => CsProjectFileReader.Read(filePath));
        }

        /// <summary>
        /// Tests that if the specified directory is not found that a 
        /// DirectoryNotFoundException is thrown.
        /// </summary>
        [Fact]
        public void HandleNoProjectFileDirectory()
        {
            const string filePath = @"test\test.xml";

            Assert.Throws<DirectoryNotFoundException>(() => CsProjectFileReader.Read(filePath));
        }

        /// <summary>
        /// Tests that a CsProjectFile is created with the specified 
        /// Target Framework 
        /// </summary>
        [Fact]
        public void CanCreateCsProjectFileWithTargetFramework()
        {
            const string filePath = @"Helpers\TestProjFile.xml";

            var result = CsProjectFileReader.Read(filePath);

            // expects 'netstandard2.0' based on the test xml file
            Assert.Equal("netstandard2.0", result.TargetFramework);
        }
    }
}
