using System;
using Xunit;
using ApplicationMetadataExtensions = GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using ContextExtensions = GiG.Core.Logging.Enrichers.Context.Extensions;
using DistributedTracingExtensions = GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Unit.Enrichers
{
    [Trait("Category", "Unit")]
    public class LoggingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void EnrichWithRequestContext_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ContextExtensions.LoggingConfigurationBuilderExtensions.EnrichWithRequestContext(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void EnrichWithApplicationMetadata_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationMetadataExtensions.LoggingConfigurationBuilderExtensions.EnrichWithApplicationMetadata(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void EnrichWithActivityContext_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws < ArgumentNullException>(() => DistributedTracingExtensions.LoggingConfigurationBuilderExtensions.EnrichWithActivityContext(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}
