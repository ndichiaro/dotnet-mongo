using MongoDB.Driver;
using Tools.Net.Mongo.Core;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate
{
    public class MigrationContext : MongoDbContext
    {
        public IMongoCollection<Changelog> Changelog { get; set; }

        public MigrationContext(string connectionString) : base(connectionString)
        {
        }
    }
}
