namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// A migration operation for upgrading a database instance
    /// </summary>
    public class UpMigrationOperation : IMigrationOperation
    {
        private readonly string _connectionString;

        public UpMigrationOperation(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Executes the migrations up function to upgrade the database
        /// based on it's current changelog
        /// </summary>
        /// <returns></returns>
        public string Execute()
        {
            // 1. get all migration files from Migration directory
            // 2. check changelog for the latest migration run
            // 3. filter migration files by ones to execute
            // 4. foreach migration file
            //      a. generate c# class
            //      b. execute up funciton
            throw new System.NotImplementedException();
        }
    }
}
