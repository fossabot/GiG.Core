using GiG.Core.Orleans.Streams.Kafka.Configurations;
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
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.FromConfiguration(null, null));
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
        public void FromConfiguration_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().FromConfiguration(null));
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }
        
        [Fact]
        public void FromConfiguration_IncorrectConfigurationSectionName_ThrowsConfigurationErrorsException()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().FromConfiguration(config.GetSection("Orleans:Stream")));
            Assert.Equal("Configuration section 'Orleans:Stream' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromConfiguration_SaslEnabledButUserNameEmpty_ThrowsConfigurationErrorsException()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettingsSsl.json")
                .Build();

            //Act - Assert
            Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().FromConfiguration(config.GetSection("Orleans:Stream")));
        }

        [Fact]
        public void FromConfiguration_SaslEnableButPasswordEmpty_ThrowsConfigurationErrorsException()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettingsSsl.json")
                .Build();

            //Act  - Assert
            Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().FromConfiguration(config.GetSection("Orleans:Stream")));
        }

        [Fact]
        public void FromConfiguration_SaslEnabled_ShouldSetValuesOnKafkaStreamOptions()
        {
            //Arrange
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var expectedConfig = config.GetSection(KafkaOptions.DefaultSectionName).Get<KafkaOptions>();
            
            //Act
            var kafkaStreamOptions = new KafkaStreamOptions().FromConfiguration(config);
            
            //Assert
            Assert.NotNull(kafkaStreamOptions);
            Assert.Equal(expectedConfig.SSl.SaslUsername, kafkaStreamOptions.SaslUserName);
            Assert.Equal(expectedConfig.SSl.SaslPassword, kafkaStreamOptions.SaslPassword);
            Assert.Equal(expectedConfig.SSl.SaslMechanism, kafkaStreamOptions.SaslMechanism);
            Assert.Equal(expectedConfig.SSl.SecurityProtocol, kafkaStreamOptions.SecurityProtocol);
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