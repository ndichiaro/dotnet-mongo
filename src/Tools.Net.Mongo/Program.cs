using System;
using System.Collections.Generic;
using Tools.Net.Mongo.Handlers;

namespace Tools.Net.Mongo
{
    class Program
    {
        static void Main(string[] args)
        {
            IToolHandler optionHandler = null;

            // to do print out usage
            if (args.Length == 0)
            {
                Console.WriteLine("Run dotnet mongo --help for usage information.");
            }

            // queue up args 
            var argList = new Queue<string>(args);

            try
            {
                do
                {
                    var arg = argList.Dequeue();

                    switch (arg)
                    {
                        case "migrate":
                            optionHandler = new MigrateToolHandler();
                            break;
                        case "-h":
                        case "--help":
                            optionHandler = new HelpToolHandler();
                            break;
                        case "-v":
                        case "--version":
                            optionHandler = new VersionToolHandler();
                            break;
                        default:
                            Console.WriteLine($"{arg} is an invalid argument. Run dotnet mongo --help for usage information.");
                            argList.Clear();
                            break;
                    }
                    optionHandler?.Run(argList);
                }
                while (argList.Count != 0);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: An unexpected error occurred.");
                Console.Error.WriteLine(ex);
                Environment.ExitCode = -1;
            }

#if DEBUG
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
#endif
        }
    }
}
