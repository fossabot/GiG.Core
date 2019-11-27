using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Serializers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;

namespace GiG.Core.Messaging.Kafka.Extensions
{
    public static class KafkaOptionsExtensions
    {
        public static IKafkaBuilderOptions<TKey, TValue> WithJson<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));
            
            builderOptions.Serializers = new Serializers<TKey, TValue>
            {
                ValueSerializer = new JsonSerializer<TValue>(),
                ValueDeserializer = new JsonDeserializer<TValue>()
            };
            
            return builderOptions;
        }

        public static IKafkaBuilderOptions<TKey, TValue> FromConfiguration<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] IConfiguration configuration)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            builderOptions.KafkaProviderOptions = configuration.GetSection(KafkaProviderOptions.DefaultSectionName).Get<KafkaProviderOptions>();
            return builderOptions;
        }

        public static IKafkaBuilderOptions<TKey, TValue> WithTopic<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] string topicName)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentNullException(nameof(topicName));
            
            builderOptions.KafkaProviderOptions.Topic = topicName;
            return builderOptions;
        }
    }
}