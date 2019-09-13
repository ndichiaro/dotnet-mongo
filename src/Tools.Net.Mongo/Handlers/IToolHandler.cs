using System.Collections.Generic;

namespace Tools.Net.Mongo.Handlers
{
    /// <summary>
    /// Exposes methods to handle program tool
    /// </summary>
    public interface IToolHandler
    {
        /// <summary>
        /// Runs the tool handler
        /// </summary>
        /// <param name="args">program arguements</param>
        void Run(Queue<string> args);
    }
}
