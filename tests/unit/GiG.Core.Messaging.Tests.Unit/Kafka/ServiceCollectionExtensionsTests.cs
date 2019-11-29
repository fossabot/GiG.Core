using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Messaging.Kafka.Abstractions.Interfaces;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Providers.DateTime.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
        public void AddKafkaProducer_SetupIsCorrect_ReturnsServiceCollection()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureLogging(x => x.AddConsole())
                .ConfigureServices((h, s) =>
                {
                    s.AddCorrelationAccessor();
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
                .ConfigureLogging(x => x.AddConsole())
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