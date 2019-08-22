using MongoDB.Driver;

namespace DotNet.Mongo.Core
{
    /// <summary>
    /// A MongoDB database connection
    /// </summary>
    public class MongoDbContext : IMongoDbContext
    {
        #region Private Variables
        private readonly IMongoClient _client;
        private readonly string _databaseName;
        #endregion

        #region Properties
        /// <summary>
        /// A MongoDB database instance
        /// </summary>
        public IMongoDatabase Db => _client.GetDatabase(_databaseName);
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of MongoDbContext
        /// </summary>
        /// <param name="client">An instance of a mongo client</param>
        /// <param name="databaseName">The name of the database to connection to</param>
        public MongoDbContext(IMongoClient client, string databaseName)
        {
            _client = client;
            _databaseName = databaseName;
        }
        #endregion
    }
}
