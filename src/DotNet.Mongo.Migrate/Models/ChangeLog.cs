using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DotNet.Mongo.Migrate.Models
{
    /// <summary>
    /// A modification to a MongoDB database
    /// </summary>
    public class ChangeLog
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FileName { get; set; }
        public DateTime AppliedAt { get; set; }
    }
}
