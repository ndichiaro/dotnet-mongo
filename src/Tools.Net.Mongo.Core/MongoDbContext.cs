using MongoDB.Driver;
using System;

namespace Tools.Net.Mongo.Core
{
    /// <summary>
    /// A MongoDB database connection
    /// </summary>
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoClient _client;
        private readonly string _databaseName;

        /// <summary>
        /// A MongoDB database instance
        /// </summary>
        public IMongoDatabase Db => _client.GetDatabase(_databaseName);

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

        /// <summary>
        /// Creates an instance of MongoDbContext using a valid <see href="https://docs.mongodb.com/upcoming/reference/connection-string/">MongoDB connection string</see>
        /// </summary>
        /// <param name="connectionString">A valid MongoDB connection string 
        /// </param>
        protected MongoDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            var mongoUrl = new MongoUrl(connectionString);
            _client = new MongoClient(mongoUrl);
            _databaseName = mongoUrl.DatabaseName;

            this.Initialize(Db);

            OnModelCreating();
        }

        /// <summary>
        /// By default this method does nothing but it can be overridden to apply
        /// specific configuration to the collection models
        /// </summary>
        protected virtual void OnModelCreating()
        {

        }
    }
}
