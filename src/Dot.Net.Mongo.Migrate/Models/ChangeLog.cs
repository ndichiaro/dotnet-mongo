using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DotNet.Mongo.Migrate.Models
{
    /// <summary>
    /// A modification to a MongoDB database
    /// </summary>
    internal class ChangeLog
    {
        /// <summary>
        /// A unique identifier for the log entry
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// The name of the migration file
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The date the migration was successfully run
        /// </summary>
        public DateTime AppliedAt { get; set; }
    }
}
