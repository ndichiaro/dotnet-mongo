using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using System;
using System.Collections.Generic;

namespace DotNet.Mongo.Parsers
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
        public MigrationOptions Parse(List<string> args)
        {
            var options = new MigrationOptions();
            //var argList = args.ToList();

            for (int i = 0; i < args.Count; i++)
            {
                var arg = args[i];
                switch (arg.ToLower())
                {
                    case "-i":
                    case "--uri":
                        var parser = ArgumentParserFactory.GetInstance<UriOptions>();

                        var value = GetValueArg(args, arg);
                        options.Uri = parser.Parse(new List<string> { value });
                        break;
                    case "up":
                        options.Operation = MigrationOperation.Up;
                        break;
                    case "down":
                        options.Operation = MigrationOperation.Down;
                        break;
                    case "create":
                        options.Operation = MigrationOperation.Create;
                        options.MigrationName = GetValueArg(args, arg);
                        break;
                    case "status":
                        options.Operation = MigrationOperation.Status;
                        break;
                    default:
                        throw new NotSupportedException($"{arg} is an invalid migrate option.");
                }
            }
            return options;
        }

        /// <summary>
        /// Gets the value arg for an option
        /// </summary>
        /// <param name="args">a list of arguments</param>
        /// <param name="arg">the uri argument</param>
        private static string GetValueArg(List<string> args, string arg)
        {
            var index = args.FindIndex(x => x == arg);
            // grab the arg after the uri option for it's value
            var valueArg = args[index + 1];
            // pull the value from the args
            args.RemoveAll(x => x == valueArg);
            return valueArg;
        }
    }
}
