﻿using Cuemon.Extensions.DependencyInjection;
using Savvyio.Extensions.EFCore;

namespace Savvyio.Extensions.DependencyInjection.EFCore
{
    /// <summary>
    /// Configuration options for <see cref="IEfCoreDataStore{TMarker}"/>.
    /// </summary>
    /// <typeparam name="TMarker">The type used to mark the implementation that these options represents. Optimized for Microsoft Dependency Injection.</typeparam>
    /// <seealso cref="EfCoreDataStoreOptions" />
    /// <seealso cref="IDependencyInjectionMarker{TMarker}" />
    public class EfCoreDataStoreOptions<TMarker> : EfCoreDataStoreOptions, IDependencyInjectionMarker<TMarker>
    {
    }
}