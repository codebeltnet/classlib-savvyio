﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore.Domain
{
    /// <summary>
    /// Provides an implementation of the <see cref="EfCoreDataSource"/> that is optimized for Domain Driven Design and the concept of Aggregate Root.
    /// </summary>
    /// <seealso cref="EfCoreDataSource" />
    public class EfCoreAggregateDataSource : EfCoreDataSource
    {
        private readonly IDomainEventDispatcher _dispatcher;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataSource"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="dbContext">The <see cref="DbContext" /> to associate with this data store.</param>
        protected EfCoreAggregateDataSource(IDomainEventDispatcher dispatcher, DbContext dbContext) : base(dbContext)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreAggregateDataSource"/> class.
        /// </summary>
        /// <param name="dispatcher">The <see cref="IDomainEventDispatcher" /> that are responsible for raising domain events.</param>
        /// <param name="options">The <see cref="EfCoreDataSourceOptions"/> used to configure this instance.</param>
        public EfCoreAggregateDataSource(IDomainEventDispatcher dispatcher, EfCoreDataSourceOptions options) : base(options)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Saves the different <see cref="IRepository{TEntity,TKey}"/> implementations as one transaction towards a data store asynchronous.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>Will, just before <see cref="DbContext.SaveChangesAsync(CancellationToken)"/> is called, extract and call <see cref="IDomainEventDispatcher.RaiseAsync"/> for all aggregate roots contained within the backing <see cref="DbContext"/>.</remarks>
        public override async Task SaveChangesAsync(Action<AsyncOptions> setup = null)
        {
            if (_dispatcher != null)
            {
                await _dispatcher.RaiseManyAsync(DbContext, setup).ConfigureAwait(false);
            }
            await base.SaveChangesAsync(setup).ConfigureAwait(false);
        }
    }
}
