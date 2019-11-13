using Serilog.Events;
using System;
using Xunit;
using ApplicationMetadataExtensions = GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using Console = GiG.Core.Logging.Sinks.Console.Extensions;
using ContextExtensions = GiG.Core.Logging.Enrichers.Context.Extensions;
using DistributedTracingExtensions = GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using File = GiG.Core.Logging.Sinks.File.Extensions;
using Fluentd = GiG.Core.Logging.Sinks.Fluentd.Extensions;
using MultiTenantExtensions = GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class LoggingConfigurationBuilderExtensionsTests
    {
        [Fact]
        public void WriteToConsole_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Console.LoggingConfigurationBuilderExtensions.WriteToConsole(null));
        }

        [Fact]
        public void WriteToFile_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => File.LoggingConfigurationBuilderExtensions.WriteToFile(null));
        }

        [Fact]
        public void WriteToFluentd_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Fluentd.LoggingConfigurationBuilderExtensions.WriteToFluentd(null));
        }

        [Fact]
        public void EnrichWithTenant_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => MultiTenantExtensions.LoggingConfigurationBuilderExtensions.EnrichWithTenant(null));
        }

        [Fact]
        public void EnrichWithRequestContext_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextExtensions.LoggingConfigurationBuilderExtensions.EnrichWithRequestContext(null));
        }

        [Fact]
        public void EnrichWithCorrelation_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => DistributedTracingExtensions.LoggingConfigurationBuilderExtensions.EnrichWithCorrelation(null));
        }

        [Fact]
        public void EnrichWithApplicationMetadata_LoggingConfigurationBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ApplicationMetadataExtensions.LoggingConfigurationBuilderExtensions.EnrichWithApplicationMetadata(null));
        }
    }
}
