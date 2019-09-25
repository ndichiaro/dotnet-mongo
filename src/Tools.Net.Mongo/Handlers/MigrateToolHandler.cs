using ConsoleTables;
using System;
using System.Collections.Generic;
using System.IO;
using Tools.Net.Mongo.Core.Builders;
using Tools.Net.Mongo.Extensions;
using Tools.Net.Mongo.Migrate;
using Tools.Net.Mongo.Migrate.Operations;
using Tools.Net.Mongo.Migrate.Options;
using Tools.Net.Mongo.Parsers;

namespace Tools.Net.Mongo.Handlers
{
    /// <summary>
    /// Handles the 'migrate' tool
    /// </summary>
    public class MigrateToolHandler : IToolHandler
    {
        /// <summary>
        /// Runs the migrate tool
        /// </summary>
        /// <param name = "args" > program arguements</param>
        public void Run(Queue<string> args)
        {
            var contextBuilder = new MongoDbContextBuilder();
            var executingLocation = Directory.GetCurrentDirectory();
            var projectFile = Directory.GetFiles(executingLocation, "*.csproj");

            if (projectFile.Length == 0)
            {
                throw new FileNotFoundException("Project file not found in current directory.");
            }

            var parser = ArgumentParserFactory.GetInstance<MigrationOptions>();

            // pull out arg as we pass the remaining arguments down the list
            var options = parser.Parse(args);

            var isValid = options.Validate();
            if (!isValid)
            {
                Console.WriteLine("Run dotnet mongo --help for usage information.");
            }

            options.ProjectFile = projectFile[0];

            var migrationResult = MigrationRunner.Run(options, contextBuilder);

            if (migrationResult.Operation == MigrationOperation.Status && migrationResult.IsSuccessful)
            {
                var table = migrationResult.Result.BuildConsoleTable();
                table.Write(Format.Alternative);
            }
            else
            {
                Console.WriteLine(migrationResult.Result);
            }
        }
    }
}
