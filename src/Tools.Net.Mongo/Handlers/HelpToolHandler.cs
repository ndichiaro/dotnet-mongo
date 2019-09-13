using System;
using System.Collections.Generic;
using System.Text;

namespace Tools.Net.Mongo.Handlers
{
    /// <summary>
    /// Handles the 'help' tool
    /// </summary>
    public class HelpToolHandler : IToolHandler
    {
        /// <summary>
        /// Runs the help tool
        /// </summary>
        /// <param name="args"></param>
        public void Run(Queue<string> args)
        {
            var helpText = new StringBuilder();

            helpText.AppendLine("Usage: dotnet mongo [tool-options] [tool] [command] [command-options]");
            helpText.AppendLine("");
            helpText.AppendLine("A Global Tool for the dotnet CLI to manage MongoDB databases in .NET.");
            helpText.AppendLine("");
            helpText.AppendLine("Tools:");
            helpText.AppendLine("  migrate [tool-options] <command>        Manages MongoDB migrations");
            helpText.AppendLine("");
            helpText.AppendLine("Tool Options:");
            helpText.AppendLine("  --help                                  Prints usage information");
            helpText.AppendLine("");
            helpText.AppendLine("Commands:");
            helpText.AppendLine("  create <name>                           Creates a new migration file. NAME is required to create a migration");
            helpText.AppendLine("  up [command-options]                    Runs all migrations that have not been applied");
            helpText.AppendLine("  down [command-options]                  Downgrades the database by undoing the last applied migration");
            helpText.AppendLine("  status [command-options]                Prints the changelog of the database");
            helpText.AppendLine("");
            helpText.AppendLine("Command Options:");
            helpText.AppendLine("  -i|--uri                                The MongoDB connection string");
            ;

            Console.WriteLine(helpText);
        }
    }
}
