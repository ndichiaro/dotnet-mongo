using MongoDB.Driver;

namespace Tools.Net.Mongo.Core
{
    /// <summary>
    /// Represents a MongoDB database connection
    /// </summary>
    public interface IMongoDbContext
    {
        /// <summary>
        /// A MongoDB database instance
        /// </summary>
        IMongoDatabase Db { get; }
    }
}
