using GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Extensions;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.OpenTelemetry.Exporters.Zipkin.Test.Unit.Tests
{
    [Trait("Category", "Unit")]
    public class TracingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void RegisterZipkin_TracerBuilderIsNull_ThrowsArgumentNullException()
        {
            // Act
            var exception = Assert.Throws<ArgumentNullException>(() => TracingConfigurationBuilderExtensions.RegisterZipkin(null));
            
            // Assert
            Assert.Equal("builder", exception.ParamName);
        }
    }
}