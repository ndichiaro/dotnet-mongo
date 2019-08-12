﻿namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// Represents a migration operation to be executed for MongoDB migration
    /// </summary>
    public interface IMigrationOperation
    {
        /// <summary>
        /// Executes the migration operation
        /// </summary>
        /// <returns>A descrption of the operation result</returns>
        string Execute();
    }
}
