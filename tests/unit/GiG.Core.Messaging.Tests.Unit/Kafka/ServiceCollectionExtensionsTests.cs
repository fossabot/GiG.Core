using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.Logging.All.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Providers.DateTime.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
using ServiceCollectionExtensions = GiG.Core.Messaging.Kafka.Extensions.ServiceCollectionExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.Kafka
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddKafkaProducer_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddKafkaProducer<object, object>(null, null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddKafkaProducer_SetupActionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddKafkaProducer<object, object>(null));
            Assert.Equal("setupAction", exception.ParamName);
        }

        [Fact]
        public void AddKafkaProducer_SetupIsCorrectWithConfiguration_ReturnsServiceCollection()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging()
                .ConfigureServices((h, s) =>
                {
                    s.AddActivityContextAccessor();
                    s.AddKafkaProducer<string, object>(options => options
                        .WithJson()
                        .FromConfiguration(h.Configuration)
                        .WithTopic("new-person-topic"));
                }).Build();

            var serviceProvider = host.Services;

            Assert.NotNull(serviceProvider);
            Assert.NotNull(serviceProvider.GetRequiredService<IDateTimeProvider>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaBuilderOptions<string, object>>());
            Assert.NotNull(serviceProvider.GetRequiredService<IProducerFactory>());
            Assert.NotNull(serviceProvider.GetRequiredService<IMessageFactory>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaProducer<string, object>>());
        }

        [Fact]
        public void AddKafkaProducer_SetupIsCorrectWithConfigurationSection_ReturnsServiceCollection()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging()
                .ConfigureServices((h, s) =>
                {
                    s.AddActivityContextAccessor();
                    s.AddKafkaProducer<string, object>(options => options
                        .WithJson()
                        .FromConfiguration(h.Configuration.GetSection(KafkaProviderOptions.DefaultSectionName))
                        .WithTopic("new-person-topic"));
                }).Build();

            var serviceProvider = host.Services;

            Assert.NotNull(serviceProvider);
            Assert.NotNull(serviceProvider.GetRequiredService<IDateTimeProvider>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaBuilderOptions<string, object>>());
            Assert.NotNull(serviceProvider.GetRequiredService<IProducerFactory>());
            Assert.NotNull(serviceProvider.GetRequiredService<IMessageFactory>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaProducer<string, object>>());
        }
        
        [Fact]
        public void AddKafkaConsumer_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddKafkaConsumer<object, object>(null, null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void AddKafkaConsumer_SetupActionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddKafkaConsumer<object, object>(null));
            Assert.Equal("setupAction", exception.ParamName);
        }

        [Fact]
        public void AddKafkaConsumer_SetupIsCorrect_ReturnsRequiredServices()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging()
                .ConfigureServices((h, s) =>
                {
                    s.AddKafkaConsumer<string, object>(options => options
                        .WithJson()
                        .FromConfiguration(h.Configuration)
                        .WithTopic("new-person-topic"));
                }).Build();

            var serviceProvider = host.Services;

            Assert.NotNull(serviceProvider);
            Assert.NotNull(serviceProvider.GetRequiredService<IDateTimeProvider>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaBuilderOptions<string, object>>());
            Assert.NotNull(serviceProvider.GetRequiredService<IConsumerFactory>());
            Assert.NotNull(serviceProvider.GetRequiredService<IKafkaConsumer<string, object>>());
        }
    }
}