using Domion.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Lib.Data
{
    /// <summary>
    ///     Generic repository implementation.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <typeparam name="TKey">Key property type</typeparam>
    public abstract class BaseRepository<TEntity, TKey> : IQueryManager<TEntity>, IEntityManager<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        ///     Creates the generic repository instance.
        /// </summary>
        /// <param name="dbContext">The DbContext to get the Entity Type from.</param>
        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        protected virtual DbContext DbContext => _dbContext;

        /// <summary>
        ///     Detaches the entity from the DbContext's change tracker.
        /// </summary>
        public void Detach(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Detached;
        }

        /// <summary>
        ///     Finds an entity with the given primary key values.
        ///     If an entity with the given primary key values is being tracked by the context,
        ///     then it is returned immediately without making a request to the database.
        ///     Otherwise, a query is made to the database for an entity with the given primary key values and this entity,
        ///     if found, is attached to the context and returned.
        ///     If no entity is found, then null is returned. (From de official docs)
        /// </summary>
        public virtual TEntity Find(TKey key)
        {
            return _dbContext.Find<TEntity>(key);
        }

        /// <summary>
        ///     Returns an entity object with the original values when it was last read from the database.
        ///     Does not include any navigation properties, not even collections.
        /// </summary>
        public virtual TEntity GetOriginalEntity(TEntity entity)
        {
            var entry = DbContext.Entry(entity);

            if (entry == null)
            {
                return null;
            }

            return entry.OriginalValues.ToObject() as TEntity;
        }

        /// <summary>
        ///     Returns an query expression that, when enumerated, will retrieve all objects.
        /// </summary>
        public virtual IQueryable<TEntity> Query()
        {
            return Query(null);
        }

        /// <summary>
        ///     Returns an query expression that, when enumerated, will retrieve only the objects that satisfy the where condition.
        /// </summary>
        public virtual IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> query = _dbSet;

            if (where != null)
            {
                query = query.Where(where);
            }

            return query;
        }

        /// <summary>
        ///     Saves changes from the DbContext's change tracker to the database.
        /// </summary>
        public virtual void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <summary>
        ///     Marks an entity for deletion in the DbContext's change tracker if it passes the ValidateDelete method.
        /// </summary>
        protected virtual IEnumerable<ValidationResult> TryDelete(TEntity entity)
        {
            var deleteErrors = ValidateDelete(entity);

            if (deleteErrors.Any())
            {
                return deleteErrors;
            }

            _dbSet.Remove(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        ///     Adds an entity for insertion in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual IEnumerable<ValidationResult> TryInsert(TEntity entity)
        {
            var saveErrors = ValidateSave(entity);

            if (saveErrors.Any())
            {
                return saveErrors;
            }

            _dbSet.Add(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        ///     Marks an entity for update in the DbContext's change tracker if it passes the ValidateSave method.
        /// </summary>
        protected virtual IEnumerable<ValidationResult> TryUpdate(TEntity entity)
        {
            var saveErrors = ValidateSave(entity);

            if (saveErrors.Any())
            {
                return saveErrors;
            }

            _dbSet.Update(entity);

            return Enumerable.Empty<ValidationResult>();
        }

        /// <summary>
        ///     Validates if it's ok to delete the entity from the database.
        /// </summary>
        protected virtual IEnumerable<ValidationResult> ValidateDelete(TEntity model)
        {
            yield break;
        }

        /// <summary>
        ///     Validates if it's ok to save the new or updated entity to the database.
        /// </summary>
        protected virtual IEnumerable<ValidationResult> ValidateSave(TEntity model)
        {
            yield break;
        }
    }
}
