using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Producers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class ProducerFactory : IProducerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessageFactory _messageFactory;
        private readonly IActivityContextAccessor _activityContextAccessor;
        private readonly Tracer _tracer;
        
        public ProducerFactory([NotNull] ILoggerFactory loggerFactory, [NotNull] IMessageFactory messageFactory, IActivityContextAccessor activityContextAccessor = null, TracerFactory tracerFactory = null)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _messageFactory = messageFactory ?? throw new ArgumentNullException(nameof(messageFactory));
            _activityContextAccessor = activityContextAccessor;
            _tracer = tracerFactory?.GetTracer(Constants.TracerName);
        }

        /// <inheritdoc />
        public IKafkaProducer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var options = new KafkaBuilderOptions<TKey, TValue>(_messageFactory);
            setupAction?.Invoke(options);

            return new KafkaProducer<TKey, TValue>(options, _loggerFactory.CreateLogger<KafkaProducer<TKey, TValue>>(), _activityContextAccessor, _tracer);
        }
    }
}