﻿using Savvyio.Handlers;

namespace Savvyio.EventDriven
{
    /// <summary>
    /// Specifies a handler responsible for objects that implements the <see cref="IIntegrationEvent"/> interface.
    /// </summary>
    /// <seealso cref="IFireForgetHandler{TRequest}" />
    public interface IIntegrationEventHandler : IFireForgetHandler<IIntegrationEvent>
    {
    }
}
