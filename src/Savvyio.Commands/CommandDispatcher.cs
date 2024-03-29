﻿using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Threading;
using Savvyio.Dispatchers;

namespace Savvyio.Commands
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="ICommandDispatcher" /> interface.
    /// </summary>
    /// <seealso cref="FireForgetDispatcher" />
    /// <seealso cref="ICommandDispatcher" />
    public class CommandDispatcher : FireForgetDispatcher, ICommandDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">The provider of service implementations.</param>
        public CommandDispatcher(IServiceLocator serviceLocator) : base(serviceLocator)
        {
        }

        /// <summary>
        /// Commits the specified <paramref name="request" /> using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand" /> to commit.</param>
        public void Commit(ICommand request)
        {
            Validator.ThrowIfNull(request);
            Dispatch<ICommand, ICommandHandler>(request, handler => handler.Delegates);
        }

        /// <summary>
        /// Commits the specified <paramref name="request" /> asynchronous using Fire-and-Forget/In-Only MEP.
        /// </summary>
        /// <param name="request">The <see cref="ICommand" /> to commit.</param>
        /// <param name="setup">The <see cref="AsyncOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public Task CommitAsync(ICommand request, Action<AsyncOptions> setup = null)
        {
            Validator.ThrowIfNull(request);
            return DispatchAsync<ICommand, ICommandHandler>(request, handler => handler.Delegates, setup);
        }
    }
}
