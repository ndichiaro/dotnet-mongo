namespace Tools.Net.Mongo.Core.Builders
{
    /// <summary>
    /// Exposes functionality for building a MongoDb database context
    /// </summary>
    public interface IMongoDbContextBuilder
    {
        /// <summary>
        /// Creates a MongoDb database context
        /// </summary>
        /// <param name="connectionString">The connection string to the database</param>
        /// <returns>An instance of IMongoDbContext</returns>
        IMongoDbContext Build(string connectionString);
    }
}
