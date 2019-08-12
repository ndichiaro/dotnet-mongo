using DotNet.Mongo.Migrate;
using DotNet.Mongo.Migrate.Options;
using DotNet.Mongo.Parsers;
using System;
using System.Linq;

namespace DotNet.Mongo
{
    class Program
    {
        static void Main(string[] args)
        {
            // to do print out usage
            if (args.Length == 0) Console.WriteLine("Please you a command.");

            foreach (var arg in args)
            {
                switch (arg)
                {
                    case "-m":
                    case "--migrate":
                        var parser = ArgumentParserFactory.GetInstance<MigrationOptions>();

                        var remainingArgs = args.Skip(1).ToArray();
                        var options = parser.Parse(remainingArgs);

                        var isValid = options.Validate();
                        if (!isValid) Console.WriteLine("Run dotnet mongo --help for usage information.");

                        MigrationRunner.Run(options);
                        break;
                    default:
                        Console.WriteLine("Run dotnet mongo --help for usage information.");
                        break;
                }
            }

        #if DEBUG
            Console.WriteLine("Press any key to continue...");
        #endif
        }
    }
}
