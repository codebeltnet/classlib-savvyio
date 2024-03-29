﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="IDomainEventDispatcher"/> interface.
    /// </summary>
    public static class DomainEventDispatcherExtensions
    {
        /// <summary>
        /// Raises domain events from the specified <paramref name="context"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="context">The <see cref="DbContext"/> to extract aggregate(s) and publish domain events from.</param>
        public static void RaiseMany(this IDomainEventDispatcher dispatcher, DbContext context)
        {
            var domainEvents = ExtractDomainEvents(context);
            foreach (var de in domainEvents)
            {
                dispatcher.Raise(de);
            }
        }

        /// <summary>
        /// Asynchronously raises domain events from the specified <paramref name="context"/> to handlers that implements the <see cref="IDomainEventHandler"/> interface.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher"/> to extend.</param>
        /// <param name="context">The <see cref="DbContext"/> to extract aggregate(s) and publish domain events from.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task RaiseManyAsync(this IDomainEventDispatcher dispatcher, DbContext context, Action<AsyncOptions> setup = null)
        {
            var domainEvents = ExtractDomainEvents(context);
            foreach (var de in domainEvents)
            {
                await dispatcher.RaiseAsync(de, setup).ConfigureAwait(false);
            }
        }

        private static IEnumerable<IDomainEvent> ExtractDomainEvents(DbContext context)
        {
            var entries = context.ChangeTracker.Entries<IAggregateRoot<IDomainEvent>>().Where(entry => entry.Entity.Events.Any()).ToList();
            foreach (var aggregate in entries.Select(entry => entry.Entity))
            {
                var events = aggregate.Events.ToList();
                var eventsType = events.First().GetType();
                if (eventsType.HasInterfaces(typeof(IDomainEvent))) { aggregate.RemoveAllEvents(); }
                foreach (var e in events)
                {
                    yield return e.MergeMetadata(aggregate);
                }
            }
        }
    }
}
