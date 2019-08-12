using MongoDB.Driver;

namespace DotNet.Mongo.Core
{
    /// <summary>
    /// A MongoDB database connection
    /// </summary>
    public class MongoDbContext : IMongoDbContext
    {
        #region Private Variables
        private static IMongoClient _client;
        private readonly MongoUrl _url;
        #endregion

        #region Properties
        /// <summary>
        /// A MongoDB database instance
        /// </summary>
        public IMongoDatabase Db => _client.GetDatabase(_url.DatabaseName);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of MongoDbContext
        /// </summary>
        /// <param name="connectionString">The MongoDB database connection string</param>
        public MongoDbContext(string connectionString)
        {
            _url = new MongoUrl(connectionString);
            _client = new MongoClient(_url.Url);
        }
        #endregion
    }
}
