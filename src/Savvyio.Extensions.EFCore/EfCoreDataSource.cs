﻿using System;
using System.Threading.Tasks;
using Cuemon;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IEfCoreDataSource"/> interface to support the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <seealso cref="Disposable" />
    /// <seealso cref="IEfCoreDataSource" />
    public class EfCoreDataSource : Disposable, IEfCoreDataSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSource"/> class.
        /// </summary>
        /// <param name="dbContext">The <see cref="DbContext"/> to associate with this data store.</param>
        protected EfCoreDataSource(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSource"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="IEfCoreDataSourceOptions" /> which need to be configured.</param>
        public EfCoreDataSource(IOptions<IEfCoreDataSourceOptions> setup)
        {
            DbContext = new EfCoreDbContext(setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreDataSource"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="IEfCoreDataSourceOptions" /> which need to be configured.</param>
        public EfCoreDataSource(Action<IEfCoreDataSourceOptions> setup) : this(Options.Create(setup.Configure()))
        {
        }

        /// <summary>
        /// Gets the session of this data store.
        /// </summary>
        /// <value>The session of this data store.</value>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Creates a <see cref="DbSet{TEntity}" /> that can be used to query and save instances of <typeparamref name="TEntity" />.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which a set should be returned.</typeparam>
        /// <returns>A set for the given entity type.</returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : class => DbContext.Set<TEntity>();

        /// <summary>
        /// Saves the different <see cref="IRepository{TEntity,TKey}"/> implementations as one transaction towards a data store asynchronous.
        /// </summary>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public virtual async Task SaveChangesAsync(Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            await DbContext.SaveChangesAsync(options.CancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Disposable.Dispose()"/> or <see cref="Disposable.Dispose(bool)"/> having <c>disposing</c> set to <c>true</c> and <see cref="Disposable.Disposed"/> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
            DbContext?.Dispose();
        }
    }
}