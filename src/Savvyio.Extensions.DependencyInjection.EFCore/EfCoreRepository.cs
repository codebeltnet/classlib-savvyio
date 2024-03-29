﻿using Savvyio.Extensions.DependencyInjection.Domain;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentRepository{TEntity,TKey,TMarker}"/> interface to support multiple implementations that serves as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreRepository{TEntity, TKey}" />
    /// <seealso cref="IPersistentRepository{TEntity, TKey, TMarker}" />
    public class EfCoreRepository<TEntity, TKey, TMarker> : EfCoreRepository<TEntity, TKey>, IPersistentRepository<TEntity, TKey, TMarker> where TEntity : class, IIdentity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity, TKey, TMarker}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreRepository(IEfCoreDataSource<TMarker> source) : base(source)
        {
        }
    }
}
