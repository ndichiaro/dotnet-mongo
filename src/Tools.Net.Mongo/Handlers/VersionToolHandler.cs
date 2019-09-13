using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tools.Net.Mongo.Handlers
{
    /// <summary>
    /// Handles the 'version' tool
    /// </summary>
    public class VersionToolHandler : IToolHandler
    {
        /// <summary>
        /// Runs the version tool
        /// </summary>
        /// <param name="args"></param>
        public void Run(Queue<string> args)
        {
            var version = Assembly.GetEntryAssembly()
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                .InformationalVersion;

            Console.WriteLine(version);
        }
    }
}
