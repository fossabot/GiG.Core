using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class LoggingEnricherTests
    {
        private readonly string _logMessageTest = Guid.NewGuid().ToString();
        private readonly IHost _host;
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;
        private readonly IRequestContextAccessor _requestContextAccessor;
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        private LogEvent _logEvent;

        public LoggingEnricherTests()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Mocks.MockStartup>())
                .UseApplicationMetadata()
                .ConfigureLogging(x => x
                    .WriteToSink(new DelegatingSink(WriteLog))
                    .EnrichWithApplicationMetadata()
                    .EnrichWithCorrelation()
                    .EnrichWithTenant()
                    .EnrichWithRequestContext()
                )
                .Build();

            _host.Start();
            _applicationMetadataAccessor = _host.Services.GetRequiredService<IApplicationMetadataAccessor>();
            _requestContextAccessor = _host.Services.GetRequiredService<IRequestContextAccessor>();
            _correlationContextAccessor = _host.Services.GetRequiredService<ICorrelationContextAccessor>();
        }

        [Fact]
        public void Logging_Enrichers_Validations()
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
    }
}