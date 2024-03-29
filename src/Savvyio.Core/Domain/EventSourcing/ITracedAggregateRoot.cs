﻿namespace Savvyio.Domain.EventSourcing
{
    /// <summary>
    /// Defines an Event Sourcing capable contract of an Aggregate as specified in Domain Driven Design.
    /// Implements the <see cref="IAggregateRoot{TKey}" />
    /// </summary>
    /// <typeparam name="TKey">The type of the key that uniquely identifies this aggregate.</typeparam>
    /// <seealso cref="IAggregateRoot{TKey}" />
    public interface ITracedAggregateRoot<out TKey> : IAggregateRoot<ITracedDomainEvent>, IEntity<TKey>
    {
        /// <summary>
        /// Gets the version of the Aggregate.
        /// </summary>
        /// <value>The version of the Aggregate.</value>
        long Version { get; }
    }
}
