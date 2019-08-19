using MongoDB.Driver;

namespace DotNet.Mongo.Core
{
    /// <summary>
    /// Represents a MongoDB migration
    /// </summary>
    public interface IMigration
    {
        /// <summary>
        /// Upgrades a MongoDB database
        /// </summary>
        /// <param name="database">The MongoDB database instance</param>
        bool Up(IMongoDatabase database);
        /// <summary>
        /// Downgrades a MongoDB database
        /// </summary>
        /// <param name="database">The MongoDB database instance</param>
        bool Down(IMongoDatabase database);
    }
}
