using GiG.Core.Http.DistributedTracing;
using GiG.Core.Http.MultiTenant;
using GiG.Core.Http.Tests.Integration.Mocks;
using Microsoft.AspNetCore.TestHost;
using Refit;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Constants = GiG.Core.DistributedTracing.Abstractions.Constants;

namespace GiG.Core.Http.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HttpClientFactoryTests : IClassFixture<TestFixture>
    {
        private readonly TestFixture _fixture;
   
        public HttpClientFactoryTests(TestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAsync_Create_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();

            var client = HttpClientFactory.Create(x =>
            {
                x.AddDelegatingHandler(new CorrelationContextDelegatingHandler(_fixture.ActivityContextAccessor));
                x.AddDelegatingHandler(new TenantDelegatingHandler(_fixture.TenantAccessor));
                x.AddDelegatingHandler(new LoggingDelegatingHandler());
                x.WithMessageHandler(testServer.CreateHandler());
                x.Options.WithBaseAddress(testServer.BaseAddress);
            });
            
            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(_fixture.ActivityContextAccessor.CorrelationId, correlation.First());
            Assert.Equal(_fixture.TenantAccessor.Values, tenants);
        }

        [Fact]
        public async Task GetAsync_Create_UseBaseAddressAsString_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();

            var client = HttpClientFactory.Create(x =>
            {
                x.AddDelegatingHandler(new CorrelationContextDelegatingHandler(_fixture.ActivityContextAccessor));
                x.AddDelegatingHandler(new TenantDelegatingHandler(_fixture.TenantAccessor));
                x.AddDelegatingHandler(new LoggingDelegatingHandler());
                x.WithMessageHandler(testServer.CreateHandler());
                x.Options.WithBaseAddress(testServer.BaseAddress);
            });

            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(_fixture.ActivityContextAccessor.CorrelationId, correlation.First());
            Assert.Equal(_fixture.TenantAccessor.Values, tenants);
        }

        [Fact]
        public async Task GetAsync_Create_UseBaseAddressWithRelativePath_ReturnsHttpNotFound()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();

            var client = HttpClientFactory.Create(x =>
            {
                x.AddDelegatingHandler(new CorrelationContextDelegatingHandler(_fixture.ActivityContextAccessor));
                x.AddDelegatingHandler(new TenantDelegatingHandler(_fixture.TenantAccessor));
                x.AddDelegatingHandler(new LoggingDelegatingHandler());
                x.WithMessageHandler(testServer.CreateHandler());
                x.Options.WithBaseAddress(testServer.BaseAddress.ToString(), "/relative");
            });

            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
            Assert.Null(correlation);
            Assert.NotEqual(_fixture.TenantAccessor.Values, tenants);
        }

        [Fact]
        public async Task GetAsync_GetOrAddWithType_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();
            var client = CreateClientWithType(testServer);
            var service = RestService.For<IMockRestClient>(client);
            
            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(_fixture.ActivityContextAccessor.CorrelationId, correlation.First());
            Assert.Equal(_fixture.TenantAccessor.Values, tenants);
        }

        [Fact]
        public async Task GetAsync_GetOrAddWithName_ReturnsHttpResponseMessage()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();
            var client = CreateClientWithName(testServer);
            var service = RestService.For<IMockRestClient>(client);

            // Act
            var actual = await service.GetAsync();
            actual.Headers.TryGetValues(Constants.Header, out var correlation);
            actual.Headers.TryGetValues(Core.MultiTenant.Abstractions.Constants.Header, out var tenants);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, actual.StatusCode);
            Assert.Equal(_fixture.ActivityContextAccessor.CorrelationId, correlation.First());
            Assert.Equal(_fixture.TenantAccessor.Values, tenants);
        }

        [Fact]
        public void GetAsync_GetOrAddWithType_ReturnsSameInstance()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();

            // Act
            var clientInstance1 = CreateClientWithType(testServer);
            var clientInstance2 = CreateClientWithType(testServer);

            // Assert
            Assert.Equal(clientInstance1, clientInstance2);
            Assert.NotNull(clientInstance1);
            Assert.NotNull(clientInstance2);
        }

        [Fact]
        public void GetAsync_GetOrAddWithName_ReturnsSameInstance()
        {
            // Arrange
            var testServer = _fixture.Host.GetTestServer();

            // Act
            var clientInstance1 = CreateClientWithName(testServer);
            var clientInstance2 = CreateClientWithName(testServer);

            // Assert
            Assert.Equal(clientInstance1, clientInstance2);
            Assert.NotNull(clientInstance1);
            Assert.NotNull(clientInstance2);
        }

        private HttpClient CreateClientWithName(TestServer testServer) =>
            HttpClientFactory.GetOrAdd(nameof(IMockRestClient), x =>
            {
                x.AddDelegatingHandler(new CorrelationContextDelegatingHandler(_fixture.ActivityContextAccessor));
                x.AddDelegatingHandler(new TenantDelegatingHandler(_fixture.TenantAccessor));
                x.AddDelegatingHandler(new LoggingDelegatingHandler());
                x.WithMessageHandler(testServer.CreateHandler());
                x.Options.WithBaseAddress(testServer.BaseAddress);
            });

        private HttpClient CreateClientWithType(TestServer testServer) =>
            HttpClientFactory.GetOrAdd<IMockRestClient>(x =>
            {
                x.AddDelegatingHandler(new CorrelationContextDelegatingHandler(_fixture.ActivityContextAccessor));
                x.AddDelegatingHandler(new TenantDelegatingHandler(_fixture.TenantAccessor));
                x.AddDelegatingHandler(new LoggingDelegatingHandler());
                x.WithMessageHandler(testServer.CreateHandler());
                x.Options.WithBaseAddress(testServer.BaseAddress);
            });
    }
}