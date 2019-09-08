using System;
using System.Collections.Generic;
using Tools.Net.Mongo.Migrate.Operations;
using Tools.Net.Mongo.Migrate.Options;

namespace Tools.Net.Mongo.Parsers
{
    /// <summary>
    /// A parser for the command line arguments for the mongo migrate option
    /// </summary>
    public class MigrateArgumentParser : IArgumentParser<MigrationOptions>
    {
        /// <summary>
        /// Parses the command line arguments
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <returns>An object representation the migration</returns>
        public MigrationOptions Parse(Queue<string> args)
        {
            var options = new MigrationOptions();

            do
            {
                var arg = args.Dequeue();
                switch (arg.ToLower())
                {
                    case "-i":
                    case "--uri":
                        var parser = ArgumentParserFactory.GetInstance<UriOptions>();
                        options.Uri = parser.Parse(args);
                        break;
                    case "up":
                        options.Operation = MigrationOperation.Up;
                        break;
                    case "down":
                        options.Operation = MigrationOperation.Down;
                        break;
                    case "create":
                        options.Operation = MigrationOperation.Create;
                        options.MigrationName = args.Dequeue();
                        break;
                    case "status":
                        options.Operation = MigrationOperation.Status;
                        break;
                    default:
                        throw new NotSupportedException($"{arg} is an invalid migrate option.");
                }
            }
            while (args.Count != 0);

            return options;
        }
    }
}
