using MongoDB.Driver;

namespace DotNet.Mongo.Migrate.Options
{
    /// <summary>
    /// Options used for configuring the MongoDB URI
    /// </summary>
    public class UriOptions
    {
        public UriOptions(string connectionString)
        {
            ConnectionString = new MongoUrl(connectionString);
        }

        /// <summary>
        /// The URI value
        /// </summary>
        public MongoUrl ConnectionString { get; }
    }
}
