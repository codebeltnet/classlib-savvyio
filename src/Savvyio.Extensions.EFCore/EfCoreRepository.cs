using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cuemon.Extensions;
using Cuemon.Threading;
using Microsoft.EntityFrameworkCore;
using Savvyio.Domain;

namespace Savvyio.Extensions.EFCore
{
    /// <summary>
    /// Provides a default implementation of the <see cref="IPersistentRepository{TEntity,TKey}"/> interface to serve as an abstraction layer before the actual I/O communication with a source of data using Microsoft Entity Framework Core.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <typeparam name="TKey">The type of the key that uniquely identifies the entity.</typeparam>
    /// <seealso cref="IPersistentRepository{TEntity, TKey}" />
    public class EfCoreRepository<TEntity, TKey> : IPersistentRepository<TEntity, TKey> where TEntity : class, IIdentity<TKey>
    {
        private readonly DbSet<TEntity> _dbSet;

        /// <summary>
        /// Initializes a new instance of the <see cref="EfCoreRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="source">The <see cref="IEfCoreDataSource"/> that handles actual I/O communication with a source of data.</param>
        public EfCoreRepository(IEfCoreDataSource source)
        {
            _dbSet = source.Set<TEntity>();
        }

        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be added in the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to add.</param>
        public void AddRange(IEnumerable<TEntity> entities)
        {
            _dbSet.AddRange(entities);
        }

        /// <summary>
        /// Loads the entity from the specified <paramref name="id"/> asynchronous.
        /// </summary>
        /// <param name="id">The key that uniquely identifies the entity.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the entity of the operation or <c>null</c> if not found.</returns>
        public Task<TEntity> GetByIdAsync(TKey id, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            return _dbSet.FindAsync(new object[] { id }, options.CancellationToken).AsTask();
        }

        /// <summary>
        /// Finds all entities matching the specified <paramref name="predicate"/> asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate that matches the entities to retrieve.</param>
        /// <param name="setup">The <see cref="AsyncOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the asynchronous operation. The task result either contains the matching entities of the operation or an empty sequence if no match was found.</returns>
        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate = null, Action<AsyncOptions> setup = null)
        {
            var options = setup.Configure();
            var dbValues = await (predicate == null ? _dbSet.ToListAsync(options.CancellationToken).ConfigureAwait(false) : _dbSet.Where(predicate).ToListAsync(options.CancellationToken).ConfigureAwait(false));
            return dbValues.Any() 
                ? dbValues 
                : predicate == null ? _dbSet.Local.ToList() : _dbSet.Local.Where(predicate.Compile()).ToList();
        }

        /// <summary>
        /// Marks the specified <paramref name="entity"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entity">The entity to remove.</param>
        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Marks the specified <paramref name="entities"/> to be removed from the data store when <see cref="IUnitOfWork.SaveChangesAsync"/> is called.
        /// </summary>
        /// <param name="entities">The entities to remove.</param>
        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
