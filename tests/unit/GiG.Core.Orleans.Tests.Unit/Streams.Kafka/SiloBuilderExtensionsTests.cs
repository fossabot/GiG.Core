using System;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.Hosting;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Streams.Kafka
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddKafkaStreamProvider_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception =
                Assert.Throws<ArgumentNullException>(() =>
                    SiloBuilderExtensions.AddKafkaStreamProvider(null, null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddKafkaStreamProvider_ProviderNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) => { sb.AddKafkaStreamProvider(null, null); })
                .Build());

            Assert.Equal("providerName", exception.ParamName);
        }
        
        [Fact]
        public void AddKafkaStreamProvider_KafkaBuilderConfigIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) => { sb.AddKafkaStreamProvider("providerName", null); })
                .Build());
            
            Assert.Equal("kafkaBuilderConfig", exception.ParamName);
        }
    }
}