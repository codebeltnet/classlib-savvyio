﻿using System;
using Cuemon;
using Cuemon.Security.Cryptography;
using Savvyio.Messaging;
using Savvyio.Messaging.Cryptography;

namespace Savvyio.EventDriven.Messaging.Cryptography
{
    /// <summary>
    /// Extension methods for the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    public static class IntegrationEventExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="event"/> to an <see cref="ISignedMessage{T}"/> equivalent using the provided <paramref name="setup"/> configurator.
        /// </summary>
        /// <typeparam name="T">The type of the payload constraint to the <see cref="IIntegrationEvent"/> interface.</typeparam>
        /// <param name="event">The <see cref="IIntegrationEvent"/> message to cryptographically sign.</param>
        /// <param name="setup">The <see cref="SignedMessageOptions{T}" /> which may be configured.</param>
        /// <returns>An implementation of the <see cref="ISignedMessage{T}"/> interface having a constraint to the <see cref="IIntegrationEvent"/> interface.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="event"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="SignedMessageOptions{T}"/> in a valid state.
        /// </exception>
        public static ISignedMessage<T> Sign<T>(this IMessage<T> @event, Action<SignedMessageOptions<T>> setup = null) where T : IIntegrationEvent
        {
            Validator.ThrowIfNull(@event);
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var message = options.SerializerFactory(@event);
            var signature = KeyedHashFactory.CreateHmacCrypto(options.SignatureSecret, options.SignatureAlgorithm).ComputeHash(message).ToHexadecimalString();
            return new SignedMessage<T>(@event, signature);
        }
    }
}