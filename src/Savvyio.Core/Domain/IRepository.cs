﻿namespace Savvyio.Domain
{
    /// <summary>
    /// A marker interface that specifies an abstraction of data persistence based on the Repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    public interface IRepository<in TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
    }
}
