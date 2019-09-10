using System.Collections.Generic;

namespace Tools.Net.Mongo.Handlers
{
    /// <summary>
    /// Exposes methods to handle program options
    /// </summary>
    public interface IOptionHandler
    {
        /// <summary>
        /// Runs the option handler
        /// </summary>
        /// <param name="args">program arguements</param>
        void Run(Queue<string> args);
    }
}
