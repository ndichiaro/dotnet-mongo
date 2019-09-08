using System;
using Tools.Net.Mongo.Migrate.Options;

namespace Tools.Net.Mongo.Parsers
{
    /// <summary>
    /// A factory for getting an instance of IArgumentParser
    /// </summary>
    public static class ArgumentParserFactory
    {
        /// <summary>
        /// Creates an instance of IArgumentParser
        /// </summary>
        /// <typeparam name="T">The result type of the argument parser</typeparam>
        /// <returns>An instance of IArgumentParser</returns>
        public static IArgumentParser<T> GetInstance<T>()
        {
            Type type = typeof(T);
            if (type == typeof(MigrationOptions))
                return (IArgumentParser<T>)new MigrateArgumentParser();

            if (type == typeof(UriOptions))
                return (IArgumentParser<T>)new UriArgumentParser();

            throw new NotSupportedException($"{type.Name} is an invalid parser type.");
        }
    }
}
