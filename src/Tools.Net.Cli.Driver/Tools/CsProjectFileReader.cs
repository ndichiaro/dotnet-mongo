using Tools.Net.Cli.Driver.Results;
using System.Xml;

namespace Tools.Net.Cli.Driver.Tools
{
    /// <summary>
    /// Reads a C# project file
    /// </summary>
    public static class CsProjectFileReader
    {
        /// <summary>
        /// Reads a C# project file
        /// </summary>
        /// <param name="filePath">The absolute path to the C# project file</param>
        /// <returns>The C# project file</returns>
        public static CsProjectFile Read(string filePath)
        {
            var projectFile = new CsProjectFile();
            var xmldoc = new XmlDocument();
            xmldoc.Load(filePath);

            // map all propery groups to file
            foreach (XmlNode propertyGroup in xmldoc.SelectNodes("//PropertyGroup"))
            {
                foreach (XmlNode node in propertyGroup.ChildNodes)
                {
                    typeof(CsProjectFile).GetProperty(node.Name)?.SetValue(projectFile, node.InnerText);
                }
            }
            return projectFile;
        }
    }
}
