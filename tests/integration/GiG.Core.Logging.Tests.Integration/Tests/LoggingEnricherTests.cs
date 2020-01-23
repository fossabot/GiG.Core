using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.File.Extensions;
using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Helpers;
using GiG.Core.Web.Mock.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public sealed class LoggingEnricherTests : IDisposable
    {
        private readonly string _logMessageTest = Guid.NewGuid().ToString();
        private readonly IHost _host;
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;
        private readonly IRequestContextAccessor _requestContextAccessor;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;
        private readonly IActivityContextAccessor _activityContextAccessor;

        private LogEvent _logEvent;

        public LoggingEnricherTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(x =>
                {
                    x.AddMockCorrelationAccessor();
                    x.AddMockTenantAccessor();
                    x.AddMockRequestContextAccessor();
                    x.AddMockActivityContextAccessor();

                })
                .UseApplicationMetadata()
                .ConfigureLogging(x => x
                    .WriteToFile()
                    .WriteToSink(new DelegatingSink(WriteLog))
                    .EnrichWithApplicationMetadata()
                    .EnrichWithActivityContext()
                    .EnrichWithCorrelation()
                    .EnrichWithTenant()
                    .EnrichWithRequestContext()
                )
                .Build();

            _host.Start();

            _applicationMetadataAccessor = _host.Services.GetRequiredService<IApplicationMetadataAccessor>();
            _requestContextAccessor = _host.Services.GetRequiredService<IRequestContextAccessor>();
            _correlationContextAccessor = _host.Services.GetRequiredService<ICorrelationContextAccessor>();
            _activityContextAccessor = _host.Services.GetRequiredService<IActivityContextAccessor>();
        }

        [Fact]
        public void LogInformation_WriteLogWithActivityContext_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingEnricherTests>>();
            const string tenantIdKey = "TenantId";

            // Act
            logger.LogInformation(_logMessageTest);
            
            // Assert
            Assert.NotNull(_logEvent);
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
        public void LogInformation_WriteLogWithEnrichers_VerifyContents()
        {
            // Arrange
            var logger = _host.Services.GetRequiredService<ILogger<LoggingEnricherTests>>();

            // Act
            logger.LogInformation(_logMessageTest);

            // Assert
            Assert.NotNull(_logEvent);
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

        private void WriteLog(LogEvent log)
        {
            if (log.MessageTemplate.Text == _logMessageTest)
            {
                _logEvent = log;
            }
        }

        public void Dispose()
        {
            _host.StopAsync().Wait();
            _host.Dispose();
        }
    }
}