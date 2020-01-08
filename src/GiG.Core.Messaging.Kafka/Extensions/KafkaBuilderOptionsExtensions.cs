using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Serializers;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using System;

namespace GiG.Core.Messaging.Kafka.Extensions
{
    /// <summary>
    /// Kafka Builder Options Extensions.
    /// </summary>
    public static class KafkaBuilderOptionsExtensions
    {
        /// <summary>
        /// Sets the ValueSerializer and ValueDeserializer to use JSON.
        /// </summary>
        /// <param name="builderOptions">The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to build upon.</param>
        /// <typeparam name="TKey">The Key value of the message.</typeparam>
        /// <typeparam name="TValue">The Value of the message.</typeparam>
        /// <returns>The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to allow chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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

        /// <summary>
        /// Configure the Kafka Provider Options from configuration.
        /// </summary>
        /// <param name="builderOptions">The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to build upon.</param>
        /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
        /// <typeparam name="TKey">The Key value of the message.</typeparam>
        /// <typeparam name="TValue">The Value of the message.</typeparam>
        /// <returns>The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to allow chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKafkaBuilderOptions<TKey, TValue> FromConfiguration<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] IConfiguration configuration)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            var configurationSection = configuration.GetSection(KafkaProviderOptions.DefaultSectionName);
            builderOptions.KafkaProviderOptions = configurationSection?.Get<KafkaProviderOptions>() ?? new KafkaProviderOptions();
            return builderOptions;
        }

        /// <summary>
        /// Configure the Kafka Provider Options from configuration.
        /// </summary>
        /// <param name="builderOptions">The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to build upon.</param>
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/>.</param>
        /// <typeparam name="TKey">The Key value of the message.</typeparam>
        /// <typeparam name="TValue">The Value of the message.</typeparam>
        /// <returns>The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to allow chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKafkaBuilderOptions<TKey, TValue> FromConfiguration<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] IConfigurationSection configurationSection)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));

            builderOptions.KafkaProviderOptions = configurationSection?.Get<KafkaProviderOptions>() ?? new KafkaProviderOptions();
            
            return builderOptions;
        }
        
        /// <summary>
        /// Sets the Kafka Topic.
        /// </summary>
        /// <param name="builderOptions">The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to build upon.</param>
        /// <param name="topicName">The topic name.</param>
        /// <typeparam name="TKey">The Key value of the message.</typeparam>
        /// <typeparam name="TValue">The Value of the message.</typeparam>
        /// <returns>The <see cref="IKafkaBuilderOptions{TKey, TValue}"/> to allow chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKafkaBuilderOptions<TKey, TValue> WithTopic<TKey, TValue>([NotNull] this IKafkaBuilderOptions<TKey, TValue> builderOptions, [NotNull] string topicName)
        {
            if (builderOptions == null) throw new ArgumentNullException(nameof(builderOptions));
            if (string.IsNullOrWhiteSpace(topicName)) throw new ArgumentException($"'{nameof(topicName)}' must not be null, empty or whitespace.", nameof(topicName));
            
            builderOptions.KafkaProviderOptions.Topic = topicName;
            return builderOptions;
        }
    }
}