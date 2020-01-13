using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.OpenTelemetry.Tests.Unit.Tests
{
    [Trait("Category", "Integration")]
    public class TracingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureTracing_TracerBuilderIsNull_ThrowsArgumentNullException()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => TracerBuilderExtensions.ConfigureTracing(null, null, null, null));
            
            // Assert
            Assert.Equal("tracerBuilder", exception.ParamName);
        }
    }
}