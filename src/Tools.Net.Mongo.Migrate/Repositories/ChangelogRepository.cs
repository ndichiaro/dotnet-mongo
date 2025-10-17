using MongoDB.Driver;
using System.Collections.Generic;
using Tools.Net.Mongo.Migrate.Models;

namespace Tools.Net.Mongo.Migrate.Repositories;

internal interface IChangelogRepository
{
    List<Changelog> All();
    long Delete(Changelog changelog);
    Changelog Insert(Changelog changelog);
}

/// <summary>
/// Manages the interaction for the changelog MongoDB
/// collection
/// </summary>
internal class ChangelogRepository : IChangelogRepository
{
    private readonly MigrationContext _context;
    /// <summary>
    /// Creates an instance of ChangeLogCollection
    /// </summary>
    /// <param name="context">MongoDB database context</param>
    public ChangelogRepository(MigrationContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all logs from the change log
    /// </summary>
    /// <returns>A list of logs</returns>
    public List<Changelog> All()
    {
        return _context.Changelog.Find(Builders<Changelog>.Filter.Empty).ToList();
    }

    /// <summary>
    /// Deletes a change log
    /// </summary>
    /// <param name="changelog">The change log to be deleted</param>
    /// <returns>1 if the change log was successfully deleted</returns>
    public long Delete(Changelog changelog)
    {
        var filterDefinition = Builders<Changelog>.Filter.Eq(x => x.Id, changelog.Id);
        var deleteResult = _context.Changelog.DeleteOne(filterDefinition);
        return deleteResult.DeletedCount;
    }

    /// <summary>
    /// Inserts a log into the change log
    /// </summary>
    /// <param name="changelog">The log being inserted</param>
    /// <returns>The inserted log updated with the database ID</returns>
    public Changelog Insert(Changelog changelog)
    {
        _context.Changelog.InsertOne(changelog);
        return changelog;
    }
}
