using ConsoleTables;
using DotNet.Mongo.Core.Builders;
using DotNet.Mongo.Extensions;
using DotNet.Mongo.Migrate;
using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using DotNet.Mongo.Parsers;
using System;
using System.Collections.Generic;
using System.IO;

namespace DotNet.Mongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var contextBuilder = new MongoDbContextBuilder();

            // to do print out usage
            if (args.Length == 0) Console.WriteLine("Run dotnet mongo --help for usage information.");

            var executingLocation = Directory.GetCurrentDirectory();
            var projectFile = Directory.GetFiles(executingLocation, "*.csproj");

            if (projectFile.Length == 0) throw new FileNotFoundException("Project file not found in current directory.");
            
            // queue up args 
            var argList = new Queue<string>(args);

            do
            {
                var arg = argList.Dequeue();

                switch (arg)
                {
                    case "-m":
                    case "--migrate":
                        var parser = ArgumentParserFactory.GetInstance<MigrationOptions>();

                        // pull out arg as we pass the remaining arguments down the list
                        var options = parser.Parse(argList);

                        var isValid = options.Validate();
                        if (!isValid) Console.WriteLine("Run dotnet mongo --help for usage information.");

                        options.ProjectFile = projectFile[0];

                        var migrationResult = MigrationRunner.Run(options, contextBuilder);
                        
                        if(migrationResult.Operation == MigrationOperation.Status)
                        {
                            var table = migrationResult.Result.BuildConsoleTable();
                            table.Write(Format.Alternative);
                        }
                        else
                        {
                            Console.WriteLine(migrationResult.Result);
                        }
                        break;
                    default:
                        Console.WriteLine($"{arg} is an invalid argument. Run dotnet mongo --help for usage information.");
                        break;
                }
            }
            while (argList.Count != 0);

        #if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        #endif
        }
    }
}
