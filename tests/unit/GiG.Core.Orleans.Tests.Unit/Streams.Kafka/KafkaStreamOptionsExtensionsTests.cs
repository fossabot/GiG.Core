using GiG.Core.Orleans.Streams.Kafka.Configurations;
using GiG.Core.Orleans.Streams.Kafka.Extensions;
using Microsoft.Extensions.Configuration;
using Orleans.Streams.Kafka.Config;
using System;
using System.Configuration;
using Xunit;
using ConfigurationSection = System.Configuration.ConfigurationSection;

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
            Assert.Equal(expectedConfig.Ssl.SaslUsername, kafkaStreamOptions.SaslUserName);
            Assert.Equal(expectedConfig.Ssl.SaslPassword, kafkaStreamOptions.SaslPassword);
            Assert.Equal(expectedConfig.Ssl.SaslMechanism, kafkaStreamOptions.SaslMechanism);
            Assert.Equal(expectedConfig.Ssl.SecurityProtocol, kafkaStreamOptions.SecurityProtocol);
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

        [Fact]
        public void AddTopicStream_KafkaStreamOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.AddTopicStream(null, null, null));
            Assert.Equal("options", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.AddTopicStream(null, null, configuration: null));
            Assert.Equal("options", exception.ParamName);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new KafkaStreamOptions().AddTopicStream(null, null));
            Assert.Equal("name", exception.ParamName);
            
            exception = Assert.Throws<ArgumentException>(() => new KafkaStreamOptions().AddTopicStream(null, configuration:null));
            Assert.Equal("name", exception.ParamName);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsNameIsWhiteSpace_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new KafkaStreamOptions().AddTopicStream("", null));
            Assert.Equal("name", exception.ParamName);
            
            exception = Assert.Throws<ArgumentException>(() => new KafkaStreamOptions().AddTopicStream("", configuration:null));
            Assert.Equal("name", exception.ParamName);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsConfigurationIsNull_ConfigurationErrorsException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new KafkaStreamOptions().AddTopicStream("Test", configuration:null));
            Assert.Equal("configuration", exception.ParamName);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsConfigurationSectionIsNull_ShouldSetDefaultValuesOnKafkaStreamOptions()
        { 
            var kafkaStreamOptions = new KafkaStreamOptions().AddTopicStream("Test", null);
            Assert.NotNull(kafkaStreamOptions);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsConfigurationNotNull_ShouldSetValuesOnKafkaStreamOptions()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            var expectedConfig = config.GetSection(KafkaTopicOptions.DefaultSectionName).Get<KafkaTopicOptions>();
            var kafkaStreamOptions = new KafkaStreamOptions().AddTopicStream("Test", config);
            
            Assert.NotNull(kafkaStreamOptions);
            Assert.Equal("Test", kafkaStreamOptions.Topics[0].Name);
            Assert.Equal(expectedConfig.Partitions, kafkaStreamOptions.Topics[0].Partitions);
            Assert.Equal(expectedConfig.ReplicationFactor, kafkaStreamOptions.Topics[0].ReplicationFactor);
            Assert.Equal(expectedConfig.RetentionPeriodInMs, kafkaStreamOptions.Topics[0].RetentionPeriodInMs);
            Assert.Equal(expectedConfig.IsTopicCreationEnabled, kafkaStreamOptions.Topics[0].AutoCreate);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsConfigurationSectionNotNull_ShouldSetValuesOnKafkaStreamOptions()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            var expectedConfig = config.GetSection(KafkaTopicOptions.DefaultSectionName).Get<KafkaTopicOptions>();
            var kafkaStreamOptions = new KafkaStreamOptions().AddTopicStream("Test", config.GetSection(KafkaTopicOptions.DefaultSectionName));
            
            Assert.NotNull(kafkaStreamOptions);
            Assert.Equal("Test", kafkaStreamOptions.Topics[0].Name);
            Assert.Equal(expectedConfig.Partitions, kafkaStreamOptions.Topics[0].Partitions);
            Assert.Equal(expectedConfig.ReplicationFactor, kafkaStreamOptions.Topics[0].ReplicationFactor);
            Assert.Equal(expectedConfig.RetentionPeriodInMs, kafkaStreamOptions.Topics[0].RetentionPeriodInMs);
            Assert.Equal(expectedConfig.IsTopicCreationEnabled, kafkaStreamOptions.Topics[0].AutoCreate);
        }
        
        [Fact]
        public void AddTopicStream_KafkaStreamsPartitionValidationFail_ConfigurationErrorsException()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var configSection = config.GetSection("Orleans:Streams:Kafka:TopicInvalidPartition");

            var exception = Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().AddTopicStream("Test", configSection));
            Assert.Equal("Number of topic partitions cannot be less than 1.", exception.Message);
        }
        
            
        [Fact]
        public void AddTopicStream_KafkaStreamsReplicationFactorValidationFail_ConfigurationErrorsException()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var configSection = config.GetSection("Orleans:Streams:Kafka:TopicInvalidReplicationFactor");

            var exception = Assert.Throws<ConfigurationErrorsException>(() => new KafkaStreamOptions().AddTopicStream("Test", configSection));
            Assert.Equal("Number of topic replicas cannot be less than 1.", exception.Message);
        }
    }
}