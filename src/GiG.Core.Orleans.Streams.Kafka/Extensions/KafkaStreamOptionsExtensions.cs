using GiG.Core.Orleans.Streams.Kafka.Configurations;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Orleans.Streams.Kafka.Config;
using System;
using System.Configuration;

namespace GiG.Core.Orleans.Streams.Kafka.Extensions
{
    /// <summary>
    ///   Kafka Streams Service Collection Extensions.
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
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection}' is incorrect.");
            }

            if (kafkaOptions.Brokers != null)
            {
                options.BrokerList = kafkaOptions.Brokers.Split(';');
            }

            options.ConsumerGroupId = kafkaOptions.ConsumerGroupId;

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
    }
}
