using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;

namespace DotNet.Mongo.Core
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
                var txtInfo = new CultureInfo("en-us", false).TextInfo;
                var titleCase = txtInfo.ToTitleCase(typeName);
                return char.ToLower(titleCase[0]) + titleCase.Substring(1);
            }
        }
        protected IMongoCollection<TEntityType> Collection => _context.Db.GetCollection<TEntityType>(CollectionName);
        #endregion

        #region Public Methods

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
