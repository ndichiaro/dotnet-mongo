using Tools.Net.Mongo.Migrate.Operations;

namespace Tools.Net.Mongo.Migrate.Options
{
    /// <summary>
    /// Options used for configuring the migration runner
    /// </summary>
    public class MigrationOptions
    {
        /// <summary>
        /// The MongoDB database uri option
        /// </summary>
        public UriOptions Uri { get; set; }

        /// <summary>
        /// The migration operation, e.g. up, down, create, status
        /// </summary>
        public MigrationOperation Operation { get; set; }

        /// <summary>
        /// The name of the migration. This is used for creating a migration
        /// </summary>
        public string MigrationName { get; set; }

        /// <summary>
        /// The absolute path of the project file
        /// </summary>
        public string ProjectFile { get; set; }

        /// <summary>
        /// Ensures the options are valid
        /// </summary>
        /// <returns>A bool indicate whether the options are valid</returns>
        public bool Validate()
        {
            if (Operation == MigrationOperation.None) return false;

            if (Operation == MigrationOperation.Create
                && string.IsNullOrEmpty(MigrationName)) return false;

            if (Operation != MigrationOperation.Create
                && Operation != MigrationOperation.Status
                && Uri == null) return false;

            return true;
        }
    }
}
