﻿using System.Collections.Generic;
using Cuemon.Configuration;
using Cuemon;
using Savvyio.Messaging;
using Amazon.SQS.Model;
using Amazon.SQS;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Savvyio.Extensions.SimpleQueueService
{
    /// <summary>
    /// Represents the base class from which all implementations of AWS SQS should derive.
    /// </summary>
    /// <typeparam name="TRequest">The type of the model to handle.</typeparam>
    /// <seealso cref="IConfigurable{TOptions}" />
    public abstract class AmazonMessage<TRequest> : IConfigurable<AmazonMessageOptions> where TRequest : IRequest
    {
        /// <summary>
        /// The key for the message attribute type.
        /// </summary>
        protected const string MessageAttributeTypeKey = "type";

        /// <summary>
        /// The maximum number of messages that AWS SQS supports when retrieving.
        /// </summary>
        protected const int AwsMaxNumberOfMessages = 10;

        /// <summary>
        /// Initializes a new instance of the <see cref="AmazonMessage{TRequest}"/> class.
        /// </summary>
        /// <param name="options">The configured <see cref="AmazonMessageOptions"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        protected AmazonMessage(AmazonMessageOptions options)
        {
            Validator.ThrowIfInvalidOptions(options, nameof(options));
            Options = options;
        }

        /// <summary>
        /// Gets the configured <see cref="AmazonMessageOptions"/> of this instance.
        /// </summary>
        /// <value>The configured <see cref="AmazonMessageOptions"/> of this instance.</value>
        public AmazonMessageOptions Options { get; }

        /// <summary>
        /// Receive one or more message(s) asynchronous from AWS SQS.
        /// </summary>
        /// <param name="setup">The <see cref="ReceiveAsyncOptions" /> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a sequence of <see cref="IMessage{T}"/> whose generic type argument is <see cref="IRequest"/>.</returns>
        protected virtual async Task<IEnumerable<IMessage<TRequest>>> RetrieveMessagesAsync(Action<ReceiveAsyncOptions> setup = null)
        {
            var options = setup.Configure();
            var sqs = new AmazonSQSClient(Options.Credentials, Options.Endpoint);

            var approximateNumberOfMessages = await FetchApproximateNumberOfMessagesAsync(sqs).ConfigureAwait(false);
            var maxNumberOfMessages = Math.Clamp(approximateNumberOfMessages, AwsMaxNumberOfMessages, options.MaxNumberOfMessages);
            var partitions = (int)Math.Ceiling(Convert.ToDouble(maxNumberOfMessages) / AwsMaxNumberOfMessages);
            
            var messages = new ConcurrentDictionary<int, IList<IMessage<TRequest>>>();
            await ParallelFactory.ForAsync(0, partitions, async (partition, ct) =>
            {
                messages.TryAdd(partition, await ProcessMessagesAsync(sqs, maxNumberOfMessages, ct).ConfigureAwait(false));
            }, o => o.CancellationToken = options.CancellationToken).ConfigureAwait(false);
            return messages.SelectMany(pair => pair.Value);
        }

        private async Task<int> FetchApproximateNumberOfMessagesAsync(IAmazonSQS sqs)
        {
            var attributes = await sqs.GetQueueAttributesAsync(new GetQueueAttributesRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                AttributeNames = new List<string>()
                {
                    "ApproximateNumberOfMessages"
                }
            }).ConfigureAwait(false);
            return attributes.ApproximateNumberOfMessages;
        }

        private async Task<IList<IMessage<TRequest>>> ProcessMessagesAsync(IAmazonSQS sqs, int maxNumberOfMessages, CancellationToken ct)
        {
            var request = new ReceiveMessageRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                MaxNumberOfMessages = maxNumberOfMessages > AwsMaxNumberOfMessages ? AwsMaxNumberOfMessages : maxNumberOfMessages,
                MessageAttributeNames = new List<string>()
                {
                    MessageAttributeTypeKey
                }
            };

            var response = await sqs.ReceiveMessageAsync(request, ct).ConfigureAwait(false);

            var messages = new List<IMessage<TRequest>>();
            foreach (var message in response.Messages)
            {
                var dataType = Type.GetType(message.MessageAttributes[MessageAttributeTypeKey].StringValue);
                var messageDataType = typeof(Message<>).MakeGenericType(dataType!);
                var deserialized = JsonFormatter.DeserializeObject(message.Body.ToStream(), messageDataType, o =>
                {
                    o.Settings.DateParseHandling = DateParseHandling.DateTime;
                    o.Settings.ContractResolver = new CamelCasePropertyNamesContractResolver
                    {
                        IgnoreSerializableInterface = true,
                        NamingStrategy = new CamelCaseNamingStrategy
                        {
                            ProcessDictionaryKeys = false
                        }
                    };
                    o.Settings.Converters.Add(DynamicJsonConverter.Create<IMetadataDictionary>(null, (reader, _, _, _) =>
                    {
                        var md = new MetadataDictionary();
                        var result = JData.ReadAll(reader);
                        foreach (var entry in result)
                        {
                            md.Add(entry.PropertyName, entry.Value);
                        }
                        return md;
                    }));
                });
                messages.Add(deserialized as IMessage<TRequest>);
            }

            if (messages.Any()) { await RemoveProcessedMessagesAsync(response, sqs, ct).ConfigureAwait(false); }

            return messages;
        }

        private Task RemoveProcessedMessagesAsync(ReceiveMessageResponse response, IAmazonSQS sqs, CancellationToken ct)
        {
            var batchRequest = new DeleteMessageBatchRequest
            {
                QueueUrl = Options.SourceQueue.OriginalString,
                Entries = new List<DeleteMessageBatchRequestEntry>(response.Messages.Select(message => new DeleteMessageBatchRequestEntry
                {
                    Id = message.MessageId,
                    ReceiptHandle = message.ReceiptHandle
                }))
            };

            return sqs.DeleteMessageBatchAsync(batchRequest, ct);
        }
    }
}