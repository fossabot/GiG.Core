﻿using GiG.Core.Messaging.Kafka.Abstractions;
using GiG.Core.Messaging.Kafka.Extensions;
using GiG.Core.Messaging.Kafka.Serializers;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.Kafka
{
    [Trait("Category", "Unit")]
    public class KafkaBuilderOptionsExtensionsTests
    {
        [Fact]
        public void WithJson_BuilderOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaBuilderOptionsExtensions.WithJson<object, object>(null));
            Assert.Equal("builderOptions", exception.ParamName);
        }
        
        [Fact]
        public void WithJson_BuilderOptionsIsValid_ReturnsIKafkaBuilderOptions()
        {
            // Arrange
            var expectedValueSerializer = new JsonSerializer<object>().GetType();
            var expectedValueDeserializer = new JsonDeserializer<object>().GetType();
            
            // Act
            var builderOptions = new KafkaBuilderOptions<object, object>().WithJson();

            // Assert
            Assert.NotNull(builderOptions);
            Assert.NotNull(builderOptions.Serializers);
            Assert.NotNull(builderOptions.Serializers.ValueSerializer);
            Assert.NotNull(builderOptions.Serializers.ValueDeserializer);
            Assert.Equal(expectedValueSerializer, builderOptions.Serializers.ValueSerializer.GetType());
            Assert.Equal(expectedValueDeserializer, builderOptions.Serializers.ValueDeserializer.GetType());
        }
        
        [Fact]
        public void FromConfiguration_BuilderOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaBuilderOptionsExtensions.FromConfiguration<object, object>(null, null));
            Assert.Equal("builderOptions", exception.ParamName);
        }

        [Fact]
        public void WithTopic_BuilderOptionsIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaBuilderOptionsExtensions.WithTopic<object, object>(null, ""));
            Assert.Equal("builderOptions", exception.ParamName);
        }
        
        [Fact]
        public void WithTopic_TopicNameIsMissing_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new KafkaBuilderOptions<object, object>().WithTopic(""));
            Assert.Equal("'topicName' must not be null, empty or whitespace. (Parameter 'topicName')", exception.Message);
        }
        
        [Fact]
        public void WithTopic_TopicNameIsInput_ReturnsIKafkaBuilderOptions()
        {
            const string expectedTopicName = "topic";
            var builder = new KafkaBuilderOptions<object, object>().WithTopic(expectedTopicName);
            
            Assert.Equal(expectedTopicName, builder.KafkaProviderOptions.Topic);
        }
    }
}