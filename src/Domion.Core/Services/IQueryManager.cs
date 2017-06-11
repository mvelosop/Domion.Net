using System;
using System.Linq;
using System.Linq.Expressions;

namespace Domion.Core.Services
{
    /// <summary>
    ///     Query Interface for the EntityManager.
    /// </summary>
    public interface IQueryManager<T> where T : class
    {
        /// <summary>
        ///     Returns an query expression that, when enumerated, will retrieve all objects from the database.
        /// </summary>
        IQueryable<T> Query();

        /// <summary>
        ///     Returns an query expression that, when enumerated, will retrieve all objects from the database that satisfy the where condition.
        /// </summary>
        IQueryable<T> Query(Expression<Func<T, bool>> where);
    }
}
