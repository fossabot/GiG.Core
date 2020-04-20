using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Factories;
using GiG.Core.Providers.DateTime.Extensions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Messaging.Kafka.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the required services for a Kafka Producer.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="setupAction">The <see><cref>Action{KafkaBuilderOptions{TKey, TValue}}</cref></see> which will be used to set kafka builder options.</param>
        /// <typeparam name="TKey">The Key of the Kafka message.</typeparam>
        /// <typeparam name="TValue">The Value of the Kafka message.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddKafkaProducer<TKey, TValue>([NotNull] this IServiceCollection services, [NotNull] Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

            services.AddUtcDateTimeProvider();
            services.Configure(setupAction);
            services.TryAddSingleton<IKafkaBuilderOptions<TKey, TValue>>(sp => sp.GetRequiredService<IOptions<KafkaBuilderOptions<TKey, TValue>>>().Value);
            services.TryAddSingleton<IProducerFactory, ProducerFactory>();
            services.TryAddSingleton<IMessageFactory, MessageFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IProducerFactory>().Create(setupAction));
            return services;
        }

        /// <summary>
        /// Add the required services for a Kafka Consumer.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="setupAction">The <see><cref>Action{KafkaBuilderOptions{TKey, TValue}}</cref></see> which will be used to set kafka builder options.</param>
        /// <typeparam name="TKey">The Key of the Kafka message.</typeparam>
        /// <typeparam name="TValue">The Value of the Kafka message.</typeparam>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddKafkaConsumer<TKey, TValue>([NotNull] this IServiceCollection services, [NotNull] Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));
            
            services.AddUtcDateTimeProvider();
            services.Configure(setupAction);
            services.TryAddSingleton<IKafkaBuilderOptions<TKey, TValue>>(sp => sp.GetRequiredService<IOptions<KafkaBuilderOptions<TKey, TValue>>>().Value);
            services.TryAddSingleton<IConsumerFactory, ConsumerFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IConsumerFactory>().Create(setupAction));

            return services;
        }
    }
}