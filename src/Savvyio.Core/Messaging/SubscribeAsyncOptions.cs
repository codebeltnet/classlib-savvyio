﻿using System;
using System.Threading;
using Cuemon.Threading;

namespace Savvyio.Messaging
{
    /// <summary>
    /// Configuration options that is related to implementations of the <see cref="ISubscriber{TRequest}"/> interface.
    /// </summary>
    /// <seealso cref="AsyncOptions" />
    public class SubscribeAsyncOptions : AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscribeAsyncOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="SubscribeAsyncOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ThrowIfCancellationWasRequested"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="RemoveProcessedMessages"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public SubscribeAsyncOptions()
        {
	        RemoveProcessedMessages = true;
        }

        /// <summary>
        /// Gets or sets a value indicating whether processed messages should be removed from the broker. Default is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> if processed messages should be removed from the broker; otherwise, <c>false</c>.</value>
        public bool RemoveProcessedMessages { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to throw an <see cref="OperationCanceledException"/> (or derived thereof) if a <see cref="CancellationToken"/> was requested cancelled.
        /// </summary>
        /// <value><c>true</c> to throw an <see cref="OperationCanceledException"/> if a <see cref="CancellationToken"/> was requested cancelled; otherwise, <c>false</c>.</value>
        public bool ThrowIfCancellationWasRequested { get; set; }
    }
}
