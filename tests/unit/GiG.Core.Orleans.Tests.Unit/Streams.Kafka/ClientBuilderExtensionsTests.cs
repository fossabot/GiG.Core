using System;
using Orleans;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Orleans.Streams.Kafka.Extensions.ClientBuilderExtensions;

namespace GiG.Core.Orleans.Tests.Unit.Streams.Kafka
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddKafkaStreamProvider_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddKafkaStreamProvider(null, null, null));
            Assert.Equal("clientBuilder", exception.ParamName);
        }

        [Fact]
        public void AddKafkaStreamProvider_ProviderNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddKafkaStreamProvider(new ClientBuilder(), null, null));
            Assert.Equal("providerName", exception.ParamName);
        }
        
        [Fact]
        public void AddKafkaStreamProvider_KafkaBuilderConfigIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddKafkaStreamProvider(new ClientBuilder(), "providerName", null));
            Assert.Equal("kafkaBuilderConfig", exception.ParamName);
        }
    }
}