﻿using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="IIntegrationEventDispatcher" /> interface.
    /// </summary>
    /// <seealso cref="FireForgetDispatcher" />
    /// <seealso cref="IIntegrationEventDispatcher" />
    public class IntegrationEventDispatcher : FireForgetDispatcher, IIntegrationEventDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationEventDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">The provider of service implementations.</param>
        public IntegrationEventDispatcher(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        public void Publish(IIntegrationEvent request)
        {
            Validator.ThrowIfNull(request);
            Dispatch<IIntegrationEvent, IIntegrationEventHandler>(request, handler => handler.Delegates);
        }

        /// <summary>
        /// Publishes the specified <paramref name="request"/> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="IIntegrationEvent"/> to publish.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task PublishAsync(IIntegrationEvent request, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(request);
            return DispatchAsync<IIntegrationEvent, IIntegrationEventHandler>(request, handler => handler.Delegates, setup);
        }
    }
}
