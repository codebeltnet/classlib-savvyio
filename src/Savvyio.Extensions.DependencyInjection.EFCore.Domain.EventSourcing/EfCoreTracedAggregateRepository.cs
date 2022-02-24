﻿using Savvyio.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.Domain.EventSourcing;
using Savvyio.Extensions.DependencyInjection.Storage;
using Savvyio.Extensions.EFCore.Domain.EventSourcing;

namespace Savvyio.Extensions.DependencyInjection.EFCore.Domain.EventSourcing
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreTracedAggregateRepository{TEntity,TKey}"/> that is optimized for Domain Driven Design and Event Sourcing.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that implements the <see cref="TracedAggregateRoot{TKey}"/> interface.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <typeparam name="TMarker">The type used to mark the implementation that this repository represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreTracedAggregateRepository{TEntity, TKey}" />
    /// <seealso cref="IReadableRepository{TEntity, TKey, TMarker}" />
    /// <seealso cref="IWritableRepository{TEntity, TKey, TMarker}" />
    public class EfCoreTracedAggregateRepository<TEntity, TKey, TMarker> : EfCoreTracedAggregateRepository<TEntity, TKey>, ITracedAggregateRepository<TEntity, TKey, TMarker>
        where TEntity : class, ITracedAggregateRoot<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreTracedAggregateRepository{TEntity, TKey, TMarker}"/> class.
        /// </summary>
        /// <param name="dataStore">The <see cref="IEfCoreDataStore{TMarker}"/> that handles actual I/O communication towards a data store.</param>
        public EfCoreTracedAggregateRepository(IEfCoreDataStore<TMarker> dataStore) : base(dataStore)
        {
        }
    }
}