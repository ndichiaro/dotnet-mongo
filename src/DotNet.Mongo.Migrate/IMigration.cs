using MongoDB.Driver;

namespace DotNet.Mongo.Migrate
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
        void Up(IMongoDatabase database);
        /// <summary>
        /// Downgrades a MongoDB database
        /// </summary>
        /// <param name="database">The MongoDB database instance</param>
        void Down(IMongoDatabase database);
    }
}
