using System.Collections.Generic;
using Tools.Net.Mongo.Migrate.Options;

namespace Tools.Net.Mongo.Parsers
{
    /// <summary>
    /// A parser for the command line arguments for uri options
    /// </summary>
    public class UriArgumentParser : IArgumentParser<UriOptions>
    {
        /// <summary>
        /// Parses the command line arguments
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <returns>An object representation the uri</returns>
        public UriOptions Parse(Queue<string> args)
        {
            return new UriOptions
            {
                ConnectionString = args.Dequeue()
            };
        }
    }
}
