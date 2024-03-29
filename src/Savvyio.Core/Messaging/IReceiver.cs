﻿using System;
using System.Collections.Generic;
using Cuemon.Threading;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Defines a consumer/receiver channel used by subsystems to receive a command and perform one or more actions (e.g., change the state).
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to invoke on a handler.</typeparam>
    public interface IReceiver<out TRequest> where TRequest : IRequest
    {
        /// <summary>
        /// Receive one or more message(s) asynchronous using Point-to-Point Channel/P2P MEP.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IRequest"/>.</returns>
        IAsyncEnumerable<IMessage<TRequest>> ReceiveAsync(Action<AsyncOptions> setup = null);
    }
}
