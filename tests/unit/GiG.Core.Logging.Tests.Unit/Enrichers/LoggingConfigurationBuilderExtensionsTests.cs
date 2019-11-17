using System;
using Xunit;
using ApplicationMetadataExtensions = GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using ContextExtensions = GiG.Core.Logging.Enrichers.Context.Extensions;
using DistributedTracingExtensions = GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using MultiTenantExtensions = GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit.Enrichers
{
    [Trait("Category", "Unit")]
    public class LoggingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void EnrichWithTenant_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => MultiTenantExtensions.LoggingConfigurationBuilderExtensions.EnrichWithTenant(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void EnrichWithRequestContext_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ContextExtensions.LoggingConfigurationBuilderExtensions.EnrichWithRequestContext(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void EnrichWithCorrelation_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => DistributedTracingExtensions.LoggingConfigurationBuilderExtensions.EnrichWithCorrelation(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void EnrichWithApplicationMetadata_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationMetadataExtensions.LoggingConfigurationBuilderExtensions.EnrichWithApplicationMetadata(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
