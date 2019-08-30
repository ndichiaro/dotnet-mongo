using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public virtual long Delete(FilterDefinition<TEntityType> filterDefinition)
        {
            var result = Collection.DeleteOne(filterDefinition);
            return result.DeletedCount;
        }

        /// <summary>
        /// Inserts a document to a collection
        /// </summary>
        /// <param name="entity">The document to be inserted</param>
        public virtual void Insert(TEntityType entity)
        {
            Collection.InsertOne(entity);
        }

        /// <summary>
        /// Finds all documents in a collection
        /// </summary>
        /// <returns>All documents in a collection</returns>
        public virtual IEnumerable<TEntityType> All()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// Finds all documents in a collection based on the expression
        /// </summary>
        /// <param name="expression">The collection query</param>
        /// <returns>All documents that match the query</returns>
        public virtual IEnumerable<TEntityType> Find(Expression<Func<TEntityType, bool>> expression)
        {
            return Collection.Find(expression).ToEnumerable();
        }
        #endregion
    }
}
