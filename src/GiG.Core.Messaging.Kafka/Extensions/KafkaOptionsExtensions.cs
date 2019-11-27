using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Serializers;
using Microsoft.Extensions.Configuration;

namespace GiG.Core.Messaging.Kafka.Extensions
{
    public static class KafkaOptionsExtensions
    {
        public static IKafkaBuilderOptions<TKey, TValue> WithJson<TKey, TValue>(this IKafkaBuilderOptions<TKey, TValue> builderOptions)
        {
            builderOptions.Serializers = new Serializers<TKey, TValue>
            {
                ValueSerializer = new JsonSerializer<TValue>(),
                ValueDeserializer = new JsonDeserializer<TValue>()
            };
            
            return builderOptions;
        }

        public static IKafkaBuilderOptions<TKey, TValue> FromConfiguration<TKey, TValue>(this IKafkaBuilderOptions<TKey, TValue> builderOptions, IConfiguration configuration)
        {
            builderOptions.KafkaProviderOptions = configuration.GetSection(KafkaProviderOptions.DefaultSectionName).Get<KafkaProviderOptions>();
            return builderOptions;
        }

        public static IKafkaBuilderOptions<TKey, TValue> WithTopic<TKey, TValue>(this IKafkaBuilderOptions<TKey, TValue> builderOptions, string topicName)
        {
            // todo: Add null check
            builderOptions.KafkaProviderOptions.Topic = topicName;
            return builderOptions;
        }
    }
}