using GiG.Core.Orleans.Streams.Kafka.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Streams.Kafka.Config;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Streams.Kafka.Extensions
{
    /// <summary>
    ///   Kafka Streams Options Extensions.
    /// </summary>  
    public static class KafkaStreamOptionsExtensions
    {
        /// <summary>
        /// Use KafkaStreamOptions from configuration section.
        /// </summary>
        /// <param name="options">The <see cref="KafkaStreamOptions" /> used to configure Kafka streams.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Kafka Stream options.</param>
        /// <returns>The <see cref="KafkaStreamOptions"/> </returns>
        public static KafkaStreamOptions FromConfiguration([NotNull] this KafkaStreamOptions options, [NotNull] IConfigurationSection configurationSection)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            var kafkaOptions = configurationSection.Get<KafkaOptions>();

            if (kafkaOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is incorrect.");
            }

            if (kafkaOptions.Brokers != null)
            {
                options.BrokerList = kafkaOptions.Brokers.Split(';');
            }

            options.ConsumerGroupId = kafkaOptions.ConsumerGroupId;

            if (kafkaOptions.Security.IsEnabled)
            {
                if (string.IsNullOrWhiteSpace(kafkaOptions.Security.SaslUsername)) throw new ConfigurationErrorsException($"Security is enabled but username is empty");
                if (string.IsNullOrEmpty(kafkaOptions.Security.SaslPassword)) throw new ConfigurationErrorsException($"Security is enabled but password is empty");

                options.SecurityProtocol = kafkaOptions.Security.SecurityProtocol;
                options.SaslUserName = kafkaOptions.Security.SaslUsername;
                options.SaslPassword = kafkaOptions.Security.SaslPassword;
                options.SaslMechanism = kafkaOptions.Security.SaslMechanism;
            }

            return options;
        }

        /// <summary>
        /// Use KafkaStreamOptions from configuration.
        /// </summary>
        /// <param name="options">The <see cref="KafkaStreamOptions" /> used to configure Kafka streams.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Kafka Streams provider's configuration options.</param>
        /// <returns>The <see cref="KafkaStreamOptions"/> </returns>
        public static KafkaStreamOptions FromConfiguration([NotNull] this KafkaStreamOptions options, [NotNull] IConfiguration configuration)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return options.FromConfiguration(configuration.GetSection(KafkaOptions.DefaultSectionName));
        }

        /// <summary>
        /// Add Kafka Topic Stream from configuration.
        /// </summary>
        /// <param name="options">The <see cref="KafkaStreamOptions" /> used to configure Kafka streams.</param>
        /// <param name="name">The name of the topic.</param>
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains Kafka Topic configuration options.</param>
        /// <returns>The <see cref="KafkaStreamOptions"/>. </returns>
        public static KafkaStreamOptions AddTopicStream([NotNull] this KafkaStreamOptions options, [NotNull] string name, [NotNull] IConfiguration configuration)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return options.AddTopicStream(name, configuration.GetSection(KafkaTopicOptions.DefaultSectionName));
        }

        /// <summary>
        /// Add Kafka Topic Stream from configuration section.
        /// </summary>
        /// <param name="options">The <see cref="KafkaStreamOptions" /> used to configure Kafka streams.</param>
        /// <param name="name">The name of the topic.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains Kafka Topic configuration options.</param>
        /// <returns>The <see cref="KafkaStreamOptions"/>. </returns>
        public static KafkaStreamOptions AddTopicStream([NotNull] this KafkaStreamOptions options, [NotNull] string name, [NotNull] IConfigurationSection configurationSection)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException($"'{nameof(name)}' must not be null, empty or whitespace.", nameof(name));

            var kafkaTopicOptions = configurationSection?.Get<KafkaTopicOptions>() ?? new KafkaTopicOptions();

            if (kafkaTopicOptions.Partitions < 1) throw new ConfigurationErrorsException("Number of topic partitions cannot be less than 1.");
            if (kafkaTopicOptions.ReplicationFactor < 1) throw new ConfigurationErrorsException("Number of topic replicas cannot be less than 1.");
         
            options.AddTopic(name, new TopicCreationConfig
                {
                    Partitions = kafkaTopicOptions.Partitions,
                    AutoCreate = kafkaTopicOptions.IsTopicCreationEnabled,
                    ReplicationFactor = kafkaTopicOptions.ReplicationFactor,
                    RetentionPeriodInMs = kafkaTopicOptions.RetentionPeriodInMs
                }
            );

            return options;
        }
    }
}