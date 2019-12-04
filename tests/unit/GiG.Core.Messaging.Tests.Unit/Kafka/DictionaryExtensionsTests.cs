using GiG.Core.Messaging.Kafka.Abstractions.Extensions;
using System;
using System.Collections.Generic;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.Kafka
{
    [Trait("Category", "Unit")]
    public class DictionaryExtensionsTests
    {
        [Fact]
        public void AddOrUpdate_DictionaryIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => DictionaryExtensions.AddOrUpdate<string, object>(null, "", null));
            Assert.Equal("dictionary", exception.ParamName);
        }

        [Fact]
        public void AddOrUpdate_KeyIsStringAndNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Dictionary<string, object>().AddOrUpdate("", null));
            Assert.Equal("Missing key. (Parameter 'key')", exception.Message);
        }

        [Fact]
        public void AddOrUpdate_KeyIsObjectAndNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new Dictionary<string, object>().AddOrUpdate(null, null));
            Assert.Equal("key", exception.ParamName);
        }

        [Fact]
        public void AddOrUpdate_KeyAlreadyExists_CurrentValueShouldChange()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>
            {
                {"a", "x"},
                {"b", "y"},
                {"c", "z"}
            };
            var currentCount = dictionary.Count;

            // Act
            dictionary.AddOrUpdate("a", "test");

            // Assert
            Assert.True(dictionary.Count == currentCount);
            Assert.Contains("test", dictionary.Values);
            Assert.DoesNotContain("x", dictionary.Values);
        }
        
        [Fact]
        public void AddOrUpdate_KeyDoesNotExist_KeyAndValueAddedToDictionary()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>
            {
                {"a", "x"},
                {"b", "y"},
                {"c", "z"}
            };
            var currentCount = dictionary.Count;

            // Act
            dictionary.AddOrUpdate("d", "test");

            // Assert
            Assert.True(dictionary.Count == currentCount + 1);
            Assert.Contains("d", dictionary.Keys);
            Assert.Contains("test", dictionary.Values);
        }
        
        [Fact]
        public void ToKafkaHeaders_DictionaryIsNull_ReturnsNewHeadersInstance()
        {
            // Arrange
            Dictionary<string, string> dictionary = null;

            // Act
            var headers = dictionary.ToKafkaHeaders();

            // Assert
            Assert.NotNull(headers);
            Assert.True(headers.Count == 0);
        }
        
        [Fact]
        public void ToKafkaHeaders_DictionaryWithTwoEntries_ReturnsHeadersWithTwoItems()
        {
            // Arrange
            var dictionary = new Dictionary<string, string>
            {
                {"Header1", "HeaderValue1"},
                {"Header2", "HeaderValue2"},
            };

            // Act
            var headers = dictionary.ToKafkaHeaders();

            // Assert
            Assert.NotNull(headers);
            Assert.True(headers.Count == dictionary.Count);
        }
    }
}