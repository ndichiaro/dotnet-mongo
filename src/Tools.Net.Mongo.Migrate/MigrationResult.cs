using Tools.Net.Mongo.Migrate.Operations;

namespace Tools.Net.Mongo.Migrate
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