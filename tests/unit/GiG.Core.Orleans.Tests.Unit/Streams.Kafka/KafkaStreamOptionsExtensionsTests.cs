using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Orleans.Streams.Kafka.Config;
using System;
using System.Configuration;
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
        
        [Fact]
        public void FromConfiguration_IncorrectConfigurationSectionName_ThrowsArgumentNullException()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var exception = Assert.Throws<ArgumentNullException>(() => new KafkaStreamOptions().FromConfiguration(config.GetSection("Orleans:Stream")));
            Assert.Equal("configurationSection", exception.ParamName);
        }
        
        [Fact]
        public void FromConfiguration_CorrectSetup_ReturnsKafkaStreamOptions()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var kafkaStreamOptions = new KafkaStreamOptions().FromConfiguration(config);
            Assert.NotNull(kafkaStreamOptions);
        }
    }
}