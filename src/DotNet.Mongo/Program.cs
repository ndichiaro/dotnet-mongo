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
            if (args.Length == 0) Console.WriteLine("Run dotnet mongo --help for usage information.");

            var argList = args.ToList();
            for (int i = 0; i < argList.Count; i++)
            {
                var arg = argList[i];
                switch (arg)
                {
                    case "-m":
                    case "--migrate":
                        var parser = ArgumentParserFactory.GetInstance<MigrationOptions>();

                        // pull out arg as we pass the remaining arguments down the list
                        argList.RemoveAll(x => x == arg);
                        var options = parser.Parse(argList);

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
            Console.ReadLine();
        #endif
        }
    }
}
