using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Http.DistributedTracing;
using GiG.Core.Http.MultiTenant;
using GiG.Core.Http.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Refit;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Http.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HttpClientFactoryTests
    {
        [Fact]
        private async Task GetAsync_CreateClientFactory_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>());
            var correlationContextAccessor = new MockCorrelationContextAccessor();
            var tenantAccessor = new MockTenantAccessor();
            var client = HttpClientFactory.CreateClient(x =>
            {
                x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(correlationContextAccessor));
                x.AddHttpMessageHandler(new TenantDelegatingHandler(tenantAccessor));
                x.AddHttpMessageHandler(new LoggingDelegatingHandler(testServer.CreateHandler()));
                x.Options.WithBaseAddress(testServer.BaseAddress);
            });

            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(correlationContextAccessor.Value.ToString(), correlation.First());
            Assert.Equal(tenantAccessor.Values, tenants);
        }
        
        [Fact]
        private async Task GetAsync_CreateClientFactory_UseBaseAddressAsString_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>());
            var correlationContextAccessor = new MockCorrelationContextAccessor();
            var tenantAccessor = new MockTenantAccessor();
            var client = HttpClientFactory.CreateClient(x =>
            {
                x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(correlationContextAccessor));
                x.AddHttpMessageHandler(new TenantDelegatingHandler(tenantAccessor));
                x.AddHttpMessageHandler(new LoggingDelegatingHandler(testServer.CreateHandler()));
                x.Options.WithBaseAddress(testServer.BaseAddress.ToString());
            });

            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(correlationContextAccessor.Value.ToString(), correlation.First());
            Assert.Equal(tenantAccessor.Values, tenants);
        }
        
        [Fact]
        private async Task GetAsync_CreateClientFactory_UseBaseAddressWithRelativePath_ReturnsHttpNotFound()
        {
            // Arrange
            var testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>());
            var correlationContextAccessor = new MockCorrelationContextAccessor();
            var tenantAccessor = new MockTenantAccessor();
            var client = HttpClientFactory.CreateClient(x =>
            {
                x.AddHttpMessageHandler(new CorrelationIdDelegatingHandler(correlationContextAccessor));
                x.AddHttpMessageHandler(new TenantDelegatingHandler(tenantAccessor));
                x.AddHttpMessageHandler(new LoggingDelegatingHandler(testServer.CreateHandler()));
                x.Options.WithBaseAddress(testServer.BaseAddress.ToString(), "/relative");
            });

            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
            Assert.Equal(correlationContextAccessor.Value.ToString(), correlation.First());
            Assert.NotEqual(tenantAccessor.Values, tenants);
        }
    }
}