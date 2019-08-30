using Dot.Net.Mongo.Migrate.Operations;

namespace Dot.Net.Mongo.Migrate
{
    /// <summary>
    /// The result of a migration
    /// </summary>
    public class MigrationResult
    {
        /// <summary>
        /// The migration operation
        /// </summary>
        public MigrationOperation Operation { get; set; }

        public string Result { get; set; }
    }
}