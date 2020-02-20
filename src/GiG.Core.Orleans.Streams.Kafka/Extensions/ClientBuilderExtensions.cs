using System;
using JetBrains.Annotations;
using Orleans;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;

namespace GiG.Core.Orleans.Streams.Kafka.Extensions
{
    /// <summary>
    /// Client Builder Extensions.
    /// </summary>
    public static class ClientBuilderExtensions
    {
        /// <summary>
        /// Registers Kafka Stream Provider with given name and configurations.
        /// The KafkaStreamClientBuilder is built here.
        /// </summary>
        /// <param name="clientBuilder">The <see cref="IClientBuilder"/>.</param>
        /// <param name="providerName">The name which will be used to register the stream provider.</param>
        /// <param name="kafkaBuilderConfig">The delegate which will be used to configure <see cref="KafkaStreamClientBuilder"/>.</param>
        /// <returns></returns>
        public static IClientBuilder AddKafkaStreamProvider([NotNull] this IClientBuilder clientBuilder, [NotNull] string providerName, 
            [NotNull] Action<KafkaStreamClientBuilder> kafkaBuilderConfig)
        {
            if (clientBuilder == null) throw new ArgumentNullException(nameof(clientBuilder));
            if (string.IsNullOrWhiteSpace(providerName)) throw new ArgumentException($"'{nameof(providerName)}' must not be null, empty or whitespace.", nameof(providerName));
            if (kafkaBuilderConfig == null) throw new ArgumentNullException(nameof(kafkaBuilderConfig));
            
            var kafkaBuilder = clientBuilder.AddKafka(providerName);

            kafkaBuilderConfig(kafkaBuilder);

            return kafkaBuilder.Build();
        }
    }
}