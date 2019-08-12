namespace DotNet.Mongo.Migrate.Operations
{
    /// <summary>
    /// MongoDB migration operations
    /// </summary>
    public enum MigrationOperation
    {
        None,
        Up,
        Down,
        Status,
        Create
    }
}
