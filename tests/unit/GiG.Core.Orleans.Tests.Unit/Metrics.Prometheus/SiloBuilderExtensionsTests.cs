using GiG.Core.Orleans.Silo.Metrics.Prometheus.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Metrics.Prometheus
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddPrometheusTelemetry_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddPrometheusTelemetry(null, null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }
        
        [Fact]
        public void AddPrometheusTelemetry_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => 
                Host.CreateDefaultBuilder().UseOrleans(sb => sb.AddPrometheusTelemetry(null)).Build());
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
