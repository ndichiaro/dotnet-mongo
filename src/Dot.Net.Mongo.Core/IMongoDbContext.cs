using MongoDB.Driver;

namespace Dot.Net.Mongo.Core
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
