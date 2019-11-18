using GiG.Core.Orleans.Streams.Kafka.Extensions;
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
            var exception = Assert.Throws<ArgumentNullException>(() => KafkaStreamOptionsExtensions.FromConfiguration(null, null));
            Assert.Equal("options", exception.ParamName);
        }
    }
}