using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Factories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Messaging.Kafka.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaProducer<TKey, TValue>(this IServiceCollection services, Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            //Guard.IsNotNull(services, nameof(services));

            //services.AddDateTimeProvider();
            services.Configure(setupAction);
            services.TryAddSingleton<IKafkaBuilderOptions<TKey, TValue>>(sp => sp.GetService<IOptions<KafkaBuilderOptions<TKey, TValue>>>().Value);
            services.TryAddSingleton<IProducerFactory, ProducerFactory>();
            services.TryAddSingleton<IMessageFactory, MessageFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IProducerFactory>().Create(setupAction));
            return services;
        }

        public static IServiceCollection AddKafkaConsumer<TKey, TValue>(this IServiceCollection services, Action<KafkaBuilderOptions<TKey, TValue>> setupAction)
        {
            //Guard.IsNotNull(services, nameof(services));

            //services.AddDateTimeProvider();
            services.Configure(setupAction);
            services.TryAddSingleton<IKafkaBuilderOptions<TKey, TValue>>(sp => sp.GetService<IOptions<KafkaBuilderOptions<TKey, TValue>>>().Value);
            services.TryAddSingleton<IConsumerFactory, ConsumerFactory>();
            services.TryAddSingleton(x => x.GetRequiredService<IConsumerFactory>().Create(setupAction));

            return services;
        }
    }
}