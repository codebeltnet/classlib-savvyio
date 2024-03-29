﻿using System;
using Cuemon.Extensions;
using Savvyio.Domain.EventSourcing;

namespace Savvyio.Domain
{
    /// <summary>
    /// Extension methods for the <see cref="IDomainEvent"/> interface.
    /// </summary>
    public static class DomainEventExtensions
    {
        /// <summary>
        /// Gets the string representation of the event identifier from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The string representation of the event identifier from the <paramref name="request"/>.</returns>
        public static string GetEventId<T>(this T request) where T : IDomainEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.EventId).As<string>();
        }

        /// <summary>
        /// Gets the <see cref="DateTime"/> value of the timestamp from the <paramref name="request"/>.
        /// </summary>
        /// <typeparam name="T">The model that implements the <see cref="ITracedDomainEvent"/> interface.</typeparam>
        /// <param name="request">The <see cref="ITracedDomainEvent"/> to extend.</param>
        /// <returns>The <see cref="DateTime"/> value of the timestamp from the <paramref name="request"/>.</returns>
        public static DateTime GetTimestamp<T>(this T request) where T : IDomainEvent
        {
            return MetadataFactory.Get(request, MetadataDictionary.Timestamp).As<DateTime>();
        }
    }
}
