namespace Domion.Core.Services
{
    /// <summary>
    ///     DbContext Find Interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">Key value(s)</typeparam>
    public interface IEntityManager<T, TKey> where T : class
    {
        /// <summary>
        ///     Finds an entity with the given primary key values.
        ///     If an entity with the given primary key values is being tracked by the context,
        ///     then it is returned immediately without making a request to the database.
        ///     Otherwise, a query is made to the database for an entity with the given primary key values and this entity,
        ///     if found, is attached to the context and returned.
        ///     If no entity is found, then null is returned. (From de official docs)
        /// </summary>
        T Find(TKey key);
    }
}
