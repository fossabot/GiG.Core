using JetBrains.Annotations;
using Orleans.Hosting;
using Orleans.Streams.Kafka.Config;
using System;

namespace GiG.Core.Orleans.Streams.Kafka.Extensions
{
    /// <summary>
    /// The <see cref="ISiloBuilder" /> Extensions.
    /// </summary>
    public static class SiloBuilderExtensions
    {
        /// <summary>
        /// Registers Kafka Stream Provider with given name and configurations.
        /// </summary>
        /// <param name="builder">The <see cref="ISiloBuilder"/>.</param>
        /// <param name="providerName">The name which will be used to register the stream provider.</param>
        /// <param name="kafkaBuilderConfig">The delegate which will be used to configure <see cref="KafkaStreamSiloBuilder"/>.</param>
        /// <returns></returns>
        public static ISiloBuilder AddKafkaStreamProvider([NotNull] this ISiloBuilder builder,
            [NotNull] string providerName, [NotNull] Action<KafkaStreamSiloBuilder> kafkaBuilderConfig)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(providerName)) throw new ArgumentException($"'{nameof(providerName)}' must not be null, empty or whitespace.", nameof(providerName));
            if (kafkaBuilderConfig == null) throw new ArgumentNullException(nameof(kafkaBuilderConfig));

            var kafkaBuilder = builder.AddKafka(providerName);
            kafkaBuilderConfig(kafkaBuilder);
            
            return kafkaBuilder.Build();
        }
    }
}