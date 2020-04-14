using Confluent.Kafka;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Orleans.Streams.Kafka.Configurations;
using HealthChecks.Kafka;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;

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
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <param name="topic">The topic name to produce kafka messages on. Optional. If <c>null</c> the topic default 'healthcheck-topic' will be used for the name.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'kafka' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddKafkaOrleansStreams([NotNull] this IHealthChecksBuilder builder, 
            [NotNull] IConfiguration configuration, string topic = "OrleansStreamsHealthCheck",
            string name = "OrleansStreamsKafka", HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return builder.AddKafkaOrleansStreams(configuration.GetSection(KafkaOptions.DefaultSectionName), topic,
                name, failureStatus, tags);
        }

        /// <summary>
        /// Add a health check for Kafka cluster.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <param name="topic">The topic name to produce kafka messages on. Optional. If <c>null</c> the topic default 'healthcheck-topic' will be used for the name.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'kafka' will be used for the name.</param>
        /// <param name="failureStatus">
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// </param>
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddKafkaOrleansStreams([NotNull] this IHealthChecksBuilder builder, 
            [NotNull] IConfigurationSection configurationSection, string topic = "OrleansStreamsHealthCheck",
            string name = "OrleansStreamsKafka", HealthStatus? failureStatus = default, IEnumerable<string> tags = default)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            return builder.AddCachedCheck(name, _ =>
            {
                var streamsOptions = configurationSection.Get<KafkaOptions>();

                var producerOptions = new ProducerConfig {BootstrapServers = streamsOptions.Brokers};
                if (streamsOptions.Security.IsEnabled)
                {
                    producerOptions.SaslUsername = streamsOptions.Security.SaslUsername;
                    producerOptions.SaslPassword = streamsOptions.Security.SaslPassword;
                    producerOptions.SecurityProtocol = (SecurityProtocol) (int) streamsOptions.Security.SecurityProtocol;
                    producerOptions.SaslMechanism = (SaslMechanism) (int) streamsOptions.Security.SaslMechanism;
                }

                return new KafkaHealthCheck(producerOptions, topic);
            }, failureStatus, tags);
        }
    }
}