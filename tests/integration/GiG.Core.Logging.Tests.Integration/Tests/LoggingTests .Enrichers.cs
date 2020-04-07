using GiG.Core.Logging.Enrichers.DistributedTracing;
using GiG.Core.Logging.Tests.Integration.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    public partial class LoggingTests
    {
        [Fact]
        public async Task LogInformation_WriteLogWithActivityContext_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingTests>>();
            const string tenantIdKey = "TenantId";

            // Act
            logger.LogInformation(_logMessageTest);
            
            // Assert
            await AssertLogEventAsync();
            var traceId = _logEvent.Properties[TracingFields.TraceId].LiteralValue().ToString();
            var spanId = _logEvent.Properties[TracingFields.SpanId].LiteralValue().ToString();
            var parentSpanId = _logEvent.Properties[TracingFields.ParentId].LiteralValue().ToString();
            var baggageTenantId = _logEvent.Properties[$"{TracingFields.BaggagePrefix}{tenantIdKey}"].LiteralValue().ToString();
            
            Assert.NotNull(traceId);
            Assert.NotNull(spanId);
            Assert.NotNull(parentSpanId);
            Assert.NotNull(baggageTenantId);
            
            Assert.Equal(_activityContextAccessor.TraceId, traceId);
            Assert.Equal(_activityContextAccessor.SpanId, spanId);
            Assert.Equal(_activityContextAccessor.ParentSpanId, parentSpanId);
            Assert.Equal(
                _activityContextAccessor.Baggage.First(x => x.Key == tenantIdKey).Value, 
                baggageTenantId);
        }
        
        [Fact]
        public async Task LogInformation_WriteLogWithEnrichers_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingTests>>();

            // Act
            logger.LogInformation(_logMessageTest);

            // Assert
            await AssertLogEventAsync();
            var applicationName = (string) _logEvent.Properties["ApplicationName"].LiteralValue();
            var applicationVersion = (string) _logEvent.Properties["ApplicationVersion"].LiteralValue();
            var correlationId = (string) _logEvent.Properties["CorrelationId"].LiteralValue();
            var ipAddress = (string) _logEvent.Properties["IPAddress"].LiteralValue();
            var tenantIds = _logEvent.Properties["TenantId"].SequenceValues();

            Assert.NotNull(applicationName);
            Assert.NotNull(applicationVersion);
            Assert.NotNull(correlationId);
            Assert.NotNull(ipAddress);
            Assert.NotNull(tenantIds);

            Assert.Equal(_applicationMetadataAccessor.Name, applicationName);
            Assert.Equal(_applicationMetadataAccessor.Version, applicationVersion);
            Assert.Equal(_correlationContextAccessor.Value.ToString(), correlationId);
            Assert.Equal(_requestContextAccessor.IPAddress.ToString(), ipAddress);
            Assert.Equal(2, tenantIds.Length);
            Assert.Contains("1", tenantIds);
            Assert.Contains("2", tenantIds);
        }
    }
}