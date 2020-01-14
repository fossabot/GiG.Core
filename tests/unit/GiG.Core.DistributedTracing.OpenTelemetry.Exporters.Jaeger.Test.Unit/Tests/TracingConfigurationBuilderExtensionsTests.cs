using GiG.Core.DistributedTracing.Exporters.Jaeger.External;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Jaeger.Test.Unit.Tests
{
    [Trait("Category", "Integration")]
    public class TracingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void RegisterJaeger_TracerBuilderIsNull_ThrowsArgumentNullException()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => TracingConfigurationBuilderExtensions.RegisterJaeger(null));
            
            // Assert
            Assert.Equal("builder", exception.ParamName);
        }
    }
}