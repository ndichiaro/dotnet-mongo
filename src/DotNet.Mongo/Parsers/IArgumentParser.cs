using System.Collections.Generic;

namespace DotNet.Mongo.Parsers
{
    /// <summary>
    /// Represents a command line argument parser
    /// </summary>
    public interface IArgumentParser<TOptionsType>
    {
        /// <summary>
        /// Interprets comment line args
        /// </summary>
        /// <typeparam name="TOptionsType">The result type</typeparam>
        /// <param name="args">comment line args</param>
        /// <returns>An object that represents the args</returns>
        TOptionsType Parse(Queue<string> args);
    }
}
