using System;
using JetBrains.Annotations;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;

namespace GiG.Core.Orleans.Streams.Kafka.Extensions
{
    /// <summary>
    /// Silo Builder Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Registers Kafka Stream Provider with given name and configurations.
        /// </summary>
        /// <param name="siloBuilder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="providerName">The name which will be used to register the stream provider.</param>
        /// <param name="kafkaBuilderConfig">The delegate which will be used to configure <see cref="KafkaStreamSiloBuilder"/>.</param>
        /// <returns></returns>
        public static ISiloBuilder AddKafkaStreamProvider([NotNull] this ISiloBuilder siloBuilder, [NotNull] string providerName, 
            [NotNull] Action<KafkaStreamSiloBuilder> kafkaBuilderConfig)
        {
            if (siloBuilder == null) throw new ArgumentNullException(nameof(siloBuilder));
            if (string.IsNullOrWhiteSpace(providerName)) throw new ArgumentNullException(nameof(providerName));
            if (kafkaBuilderConfig == null) throw new ArgumentNullException(nameof(kafkaBuilderConfig));
            
            var kafkaBuilder = siloBuilder.AddKafka(providerName);

            kafkaBuilderConfig(kafkaBuilder);

            return kafkaBuilder.Build();
        }
    }
}