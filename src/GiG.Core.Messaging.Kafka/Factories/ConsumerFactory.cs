using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Consumers;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using OpenTelemetry.Trace.Configuration;
using System;
using Constants = GiG.Core.Messaging.Kafka.Abstractions.Constants;

namespace GiG.Core.Messaging.Kafka.Factories
{
    /// <inheritdoc />
    internal class ConsumerFactory : IConsumerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly Tracer _tracer;

        public ConsumerFactory([NotNull] ILoggerFactory loggerFactory, TracerFactory tracerFactory = null)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _tracer = tracerFactory?.GetTracer(Constants.TracerName);
        }

        /// <inheritdoc />
        public IKafkaConsumer<TKey, TValue> Create<TKey, TValue>(Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            var builderOptions = new KafkaBuilderOptions<TKey, TValue>();
            setupAction?.Invoke(builderOptions);

            return new KafkaConsumer<TKey, TValue>(builderOptions, _loggerFactory.CreateLogger<KafkaConsumer<TKey, TValue>>(), _tracer);
        }
    }
}
