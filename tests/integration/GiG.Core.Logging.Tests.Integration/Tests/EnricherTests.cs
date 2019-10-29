using GiG.Core.Context.Web.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
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
using Microsoft.AspNetCore.HttpOverrides;
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
    public class EnricherTests
    {
        private readonly TestServer _server;

        public LogEvent LogEvent;
        public ILogger Logger;

        public EnricherTests()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder.UseTestServer();
                    webBuilder.Configure(app =>
                    {
                        app.UseForwardedHeaders();
                        app.UseCorrelationId();
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
            Logger = host.Services.GetRequiredService<ILogger<EnricherTests>>();
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
        public async Task TestEnrichers()
        {
            // Arrange
            var client = _server.CreateClient();
            const string expectedIPAddress = "192.168.0.1";

            client.DefaultRequestHeaders.Add(ForwardedHeadersDefaults.XForwardedForHeaderName, expectedIPAddress);
            client.DefaultRequestHeaders.Add(MultiTenant.Abstractions.Constants.Header, "1");
            client.DefaultRequestHeaders.Add(MultiTenant.Abstractions.Constants.Header, "2");

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            await client.SendAsync(request);

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

            Assert.True(Guid.TryParse(correlationId, out _));
            Assert.Equal(expectedIPAddress, ipAddress);
            Assert.True(tenantIds.Length == 2);
            Assert.True(Array.Exists(tenantIds, e=>e.ToString().Equals("1")));
            Assert.True(Array.Exists(tenantIds, e=>e.ToString().Equals("2")));
        }
    }
}
