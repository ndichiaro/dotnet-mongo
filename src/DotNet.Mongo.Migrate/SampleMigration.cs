using MongoDB.Bson;
using MongoDB.Driver;

/// <summary>
/// This class was generation to create a custom MongoDB database migration
/// </summary>
namespace DotNet.Mongo.Migrate
{
    public class AddUsers_201908142247197 : IMigration
    {
        public bool Down(IMongoDatabase database)
        {
            var usersCollection = database.GetCollection<BsonDocument>("users");

            var filterDefinition = Builders<BsonDocument>.Filter.Eq("firstName", "Nick");
            filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("lastName", "DiChiaro");
            filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("email", "ndichiaro@gmail.com");

            var result = usersCollection.DeleteOne(filterDefinition);

            if (result.DeletedCount == 0) return true;
            return false;
        }

        public bool Up(IMongoDatabase database)
        {
            var usersCollection = database.GetCollection<BsonDocument>("users");
            usersCollection.InsertOne(new BsonDocument
            {
                {"firstName", "Nick"},
                {"lastName", "DiChiaro"},
                {"email", "ndichiaro@gmail.com"},
            });

            var filterDefinition = Builders<BsonDocument>.Filter.Eq("firstName", "Nick");
            filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("lastName", "DiChiaro");
            filterDefinition = filterDefinition & Builders<BsonDocument>.Filter.Eq("email", "ndichiaro@gmail.com");

            var newUser = usersCollection.Find(filterDefinition).ToList();

            if (newUser.Count == 1) return true;

            return false;
        }
    }
}
