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
            var correlationId = _logEvent.Properties[TracingFields.CorrelationId].LiteralValue().ToString();
            var traceId = _logEvent.Properties[TracingFields.TraceId].LiteralValue().ToString();
            var spanId = _logEvent.Properties[TracingFields.SpanId].LiteralValue().ToString();
            var parentSpanId = _logEvent.Properties[TracingFields.ParentId].LiteralValue().ToString();
            var baggageTenantId = _logEvent.Properties[$"{TracingFields.BaggagePrefix}{tenantIdKey}"].LiteralValue().ToString();
            
            Assert.NotNull(correlationId);
            Assert.NotNull(traceId);
            Assert.NotNull(spanId);
            Assert.NotNull(parentSpanId);
            Assert.NotNull(baggageTenantId);
            
            Assert.Equal(_activityContextAccessor.CorrelationId, correlationId);
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

            Assert.NotNull(applicationName);
            Assert.NotNull(applicationVersion);
            Assert.NotNull(correlationId);
            Assert.NotNull(ipAddress);

            Assert.Equal(_applicationMetadataAccessor.Name, applicationName);
            Assert.Equal(_applicationMetadataAccessor.Version, applicationVersion);
            Assert.Equal(_activityContextAccessor.CorrelationId, correlationId);
            Assert.Equal(_requestContextAccessor.IPAddress.ToString(), ipAddress);
        }
    }
}