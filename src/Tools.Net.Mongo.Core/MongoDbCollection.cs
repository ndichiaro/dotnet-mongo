using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tools.Net.Mongo.Core
{
    /// <summary>
    /// Represents a MongoDB collection
    /// </summary>
    /// <typeparam name="TEntityType"></typeparam>
    public abstract class MongoDbCollection<TEntityType> where TEntityType : class
    {
        #region Variables
        private readonly IMongoDbContext _context;
        #endregion

        #region Constructors
        public MongoDbCollection(IMongoDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The name of the MongoDB collection. By default, this is based on
        /// name of TEntityType in camelcase formatting.
        /// </summary>
        public virtual string CollectionName
        {
            get
            {
                var typeName = typeof(TEntityType).Name;
                return char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
            }
        }
        protected IMongoCollection<TEntityType> Collection => _context.Db.GetCollection<TEntityType>(CollectionName);
        #endregion

        #region Public Methods
        /// <summary>
        /// Finds all documents in a collection
        /// </summary>
        /// <returns>All documents in a collection</returns>
        public virtual IEnumerable<TEntityType> All()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <param name="filterDefinition">The filter used to delete a document</param>
        /// <returns>The number of documents deleted</returns>
        public virtual long Delete(FilterDefinition<TEntityType> filterDefinition)
        {
            var result = Collection.DeleteOne(filterDefinition);
            return result.DeletedCount;
        }

        /// <summary>
        /// Deletes a document by a property value
        /// </summary>
        /// <typeparam name="TValueType">The data type for the MongoDB value</typeparam>
        /// <param name="expression">An expression indicating which document property to use</param>
        /// <param name="value">The value of the property to be deleted</param>
        /// <returns>The number of documents deleted</returns>
        public virtual long Delete<TValueType>(Expression<Func<TEntityType, TValueType>> expression, TValueType value)
        {
            var filterDefinition = Builders<TEntityType>.Filter.Eq(expression, value);
            return Delete(filterDefinition);
        }

        /// <summary>
        /// Finds all documents in a collection based on the filter definition
        /// </summary>
        /// <param name="filterDefinition">The query filter</param>
        /// <returns>All documents that match the query</returns>
        public virtual IEnumerable<TEntityType> Find(FilterDefinition<TEntityType> filterDefinition)
        {
            return Collection.Find(filterDefinition).ToEnumerable();
        }

        /// <summary>
        /// Finds all documents in a collection based on the filter expression
        /// </summary>
        /// <param name="expression">The collection query</param>
        /// <returns>All documents that match the query</returns>
        public virtual IEnumerable<TEntityType> Find(Expression<Func<TEntityType, bool>> expression)
        {
            return Collection.Find(expression).ToEnumerable();
        }

        /// <summary>
        /// Inserts a document to a collection
        /// </summary>
        /// <param name="entity">The document to be inserted</param>
        /// <returns>The created document</returns>
        public virtual TEntityType Insert(TEntityType entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }

        /// <summary>
        /// Inserts a set of documents into a collection
        /// </summary>
        /// <param name="entities">A set of entities to be inserted</param>
        /// <returns>The set of entities inserted into the collection </returns>
        public virtual IEnumerable<TEntityType> Insert(IEnumerable<TEntityType> entities)
        {
            Collection.InsertMany(entities);
            return entities;
        }
        #endregion
    }
}
