﻿namespace Savvyio.Queries
{
    /// <summary>
    /// Provides a default implementation of of the <see cref="IQuery{TResult}" /> interface.
    /// </summary>
    /// <typeparam name="TResult">The type of the result to return.</typeparam>
    /// <seealso cref="IQuery{TResult}" />
    /// <seealso cref="Request" />
    public abstract record Query<TResult> : Request, IQuery<TResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Query{TResult}"/> class.
        /// </summary>
        /// <param name="metadata">The optional metadata to merge with this instance.</param>
        protected Query(IMetadata metadata = null)
        {
            this.MergeMetadata(metadata);
        }
    }
}
