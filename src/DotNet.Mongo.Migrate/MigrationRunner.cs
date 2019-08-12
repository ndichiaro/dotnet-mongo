using DotNet.Mongo.Migrate.Operations;
using DotNet.Mongo.Migrate.Options;
using System;

namespace DotNet.Mongo.Migrate
{
    public static class MigrationRunner
    {
        public static void Run(MigrationOptions options)
        {
            IMigrationOperation migrationOperation;

            switch (options.Operation)
            {
                case MigrationOperation.None:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
                case MigrationOperation.Up:
                    migrationOperation = null;
                    break;
                case MigrationOperation.Down:
                    migrationOperation = null;
                    break;
                case MigrationOperation.Status:
                    migrationOperation = null;
                    break;
                case MigrationOperation.Create:
                    migrationOperation = new CreateMigrationOperation(options.MigrationName);
                    break;
                default:
                    throw new NotSupportedException($"{options.Operation} is not a supported operation.");
            }

            migrationOperation.Execute();
        }
    }
}
