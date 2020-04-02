using Confluent.Kafka;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Configurations;
using HealthChecks.Kafka;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace GiG.Core.HealthChecks.Orleans.Streams.Kafka
{
    /// <summary>
    /// The Health Checks Builder.
    /// </summary>
    public static class HealthChecksBuilderExtensions
    {
        /// <summary>
        /// Add a health check for Kafka cluster.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="topic">The topic name to produce kafka messages on. Optional. If <c>null</c> the topic default 'healthcheck-topic' will be used for the name.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'kafka' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddKafkaStreams([NotNull] this IHealthChecksBuilder builder, string topic = "OrleansStreamsHealthCheck",
            string name = "OrleansStreamsKafka", HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.AddCachedCheck(name, serviceProvider =>
            {
                var streamsOptions = serviceProvider.GetService<IOptions<KafkaOptions>>()?.Value ?? new KafkaOptions();

                var producerOptions = new ProducerConfig {BootstrapServers = streamsOptions.Brokers,};
                if (streamsOptions.Ssl.IsEnabled)
                {
                    producerOptions.SaslUsername = streamsOptions.Ssl.SaslUsername;
                    producerOptions.SaslPassword = streamsOptions.Ssl.SaslPassword;
                    producerOptions.SecurityProtocol = (SecurityProtocol) (int) streamsOptions.Ssl.SecurityProtocol;
                    producerOptions.SaslMechanism = (SaslMechanism) (int) streamsOptions.Ssl.SaslMechanism;
                }

                return new KafkaHealthCheck(producerOptions, topic);
            }, failureStatus, tags);
        }
    }
}