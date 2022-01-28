using MongoDB.Driver;
using System;

namespace Tools.Net.Mongo.Core.Builders
{
    /// <summary>
    /// Contains functionality for creating a MongoDB database context
    /// </summary>
    public static class MongoDbContextBuilder
    {
        #region Public Methods
        /// <summary>
        /// Creates a MongoDb database context
        /// </summary>
        /// <param name="connectionString">The connection string to the database</param>
        /// <returns>An instance of IMongoDbContext</returns>
        /// <exception cref="ArgumentNullException">A connection string is required</exception>
        /// <exception cref="MongoConfigurationException">An invalid MongoDB connection string</exception>
        [Obsolete("Create a context by inheriting from MongoDbContext and calling the connection sring constructor")]
        public static IMongoDbContext Build(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            var mongoUrl = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            return new MongoDbContext(mongoClient, mongoUrl.DatabaseName);
        }
        #endregion
    }
}
