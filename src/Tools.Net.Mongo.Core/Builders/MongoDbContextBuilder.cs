using System;
using MongoDB.Driver;

namespace Tools.Net.Mongo.Core.Builders
{
    /// <summary>
    /// Contains functionality for creating a MongoDB database context
    /// </summary>
    public class MongoDbContextBuilder : IMongoDbContextBuilder
    {
        #region Public Methods
        /// <summary>
        /// Creates a MongoDb database context
        /// </summary>
        /// <param name="connectionString">The connection string to the database</param>
        /// <returns>An instance of IMongoDbContext</returns>
        ///<exception cref="ArgumentNullException">A connection string is required</exception>
        public virtual IMongoDbContext Build(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException("connectionString");

            var mongoUrl = new MongoUrl(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            return new MongoDbContext(mongoClient, mongoUrl.DatabaseName);
        }
        #endregion
    }
}
