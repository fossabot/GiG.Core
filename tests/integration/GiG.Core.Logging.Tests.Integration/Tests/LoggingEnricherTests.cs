using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Helpers;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class LoggingEnricherTests
    {
        private readonly TestServer _server;
        private readonly IApplicationMetadataAccessor _applicationMetadataAccessor;
        private readonly IRequestContextAccessor _requestContextAccessor;

        public LogEvent LogEvent;
        public ILogger Logger;

        public LoggingEnricherTests()
        {
            var host = Host.CreateDefaultBuilder()
                .UseApplicationMetadata()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.Configure(app =>
                    {
                        app.UseForwardedHeaders();
                        app.UseCorrelation();
                    });
                    webBuilder.ConfigureServices(x =>
                    {
                        x.ConfigureForwardedHeaders();
                        x.AddCorrelationAccessor();
                        x.AddTenantAccessor();
                        x.AddRequestContextAccessor();
                    });
                })
                .ConfigureLogging(x => x
                    .WriteToSink(new DelegatingSink(e => LogEvent = e))
                    .EnrichWithApplicationMetadata()
                    .EnrichWithCorrelationId()
                    .EnrichWithTenantId()
                    .EnrichWithRequestContext()
                ).Build();

            host.StartAsync().GetAwaiter().GetResult();

            _server = host.GetTestServer();
            Logger = host.Services.GetRequiredService<ILogger<LoggingEnricherTests>>();
            _applicationMetadataAccessor = host.Services.GetRequiredService<IApplicationMetadataAccessor>();
            _requestContextAccessor = host.Services.GetRequiredService<IRequestContextAccessor>();
        }

        [Fact]
        public async Task GenericCreateAndStartHost_GetTestServer()
        {
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .Configure(app => { });
                })
                .StartAsync();
  
            var response = await host.GetTestServer().CreateClient().GetAsync("/"); 
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode); 
        } 

        [Fact]
        public async Task Logging_Enrichers_Validations()
        {
            // Arrange
            var client = _server.CreateClient();
            
            client.DefaultRequestHeaders.Add(DistributedTracing.Abstractions.Constants.Header, Guid.NewGuid().ToString());
            client.DefaultRequestHeaders.Add(MultiTenant.Abstractions.Constants.Header, "1");
            client.DefaultRequestHeaders.Add(MultiTenant.Abstractions.Constants.Header, "2");

            // Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            await client.SendAsync(request);
            
            // Assert
            Assert.NotNull(LogEvent);
            var applicationName = (string)LogEvent.Properties["ApplicationName"].LiteralValue();
            var applicationVersion = (string)LogEvent.Properties["ApplicationVersion"].LiteralValue();
            var correlationId = (string)LogEvent.Properties["CorrelationId"].LiteralValue();
            var ipAddress = (string)LogEvent.Properties["IPAddress"].LiteralValue();
            var tenantIds = LogEvent.Properties["TenantId"].SequenceValues();

            Assert.NotNull(applicationName);
            Assert.NotNull(applicationVersion);
            Assert.NotNull(correlationId);
            Assert.NotNull(ipAddress);
            Assert.NotNull(tenantIds);

            Assert.Equal(_applicationMetadataAccessor.Name, applicationName);
            Assert.Equal(_applicationMetadataAccessor.Version, applicationVersion);
            Assert.True(Guid.TryParse(correlationId, out _));
            Assert.Equal(_requestContextAccessor.IPAddress.ToString(), ipAddress);
            Assert.True(tenantIds.Length == 2);
            Assert.True(Array.Exists(tenantIds, e=>e.ToString().Equals("1")));
            Assert.True(Array.Exists(tenantIds, e=>e.ToString().Equals("2")));
        }
    }
}
