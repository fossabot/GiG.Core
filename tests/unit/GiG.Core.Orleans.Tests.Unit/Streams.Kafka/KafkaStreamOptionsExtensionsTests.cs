using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Orleans.Streams.Kafka.Config;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Streams.Kafka
{
    [Trait("Category", "Unit")]
    public class KafkaStreamOptionsExtensionsTests
    {
        [Fact]
        public void FromConfiguration_KafkaStreamOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.FromConfiguration(null, configurationSection: null));
            Assert.Equal("options", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.FromConfiguration(null, configuration: null));
            Assert.Equal("options", exception.ParamName);
        }

        [Fact]
        public void FromConfiguration_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new KafkaStreamOptions().FromConfiguration(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromConfiguration_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new KafkaStreamOptions().FromConfiguration(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }
    }
}