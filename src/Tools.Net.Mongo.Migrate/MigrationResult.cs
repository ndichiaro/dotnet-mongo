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

        /// <summary>
        /// The migration result
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// Indicates if the operation was successful
        /// </summary>
        public bool IsSuccessful { get; set; }
    }
}