using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.MultiTenant.Web.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.MultiTenant.Web.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class TenantAccessorTests
    {
        private readonly TestServer _server;

        public TenantAccessorTests()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<MockStartup>());
        }

        [Fact]
        public async Task TenantIdValue_TenantIdMatchesRequestHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            const string tenantId = "1";
            client.DefaultRequestHeaders.Add(Constants.Header, tenantId);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var headerValues = JsonSerializer.Deserialize<IImmutableSet<string>>(content);

            // Assert
            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Equal(tenantId, headerValues.First());
        }

        [Fact]
        public async Task TenantIdValue_DuplicateTenantToSingleRequestHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            const string tenantId = "1";
            client.DefaultRequestHeaders.Add(Constants.Header, tenantId);
            client.DefaultRequestHeaders.Add(Constants.Header, tenantId);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var headerValues = JsonSerializer.Deserialize<IImmutableSet<string>>(content);

            // Assert
            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Single(headerValues);
            Assert.Equal(tenantId, headerValues.First());
        }

        [Fact]
        public async Task TenantIdValue_DifferentTenantsInRequestHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            var listTenants =  new List<string> { "1", "2" };
   
            client.DefaultRequestHeaders.Add(Constants.Header, listTenants.First());
            client.DefaultRequestHeaders.Add(Constants.Header, listTenants.Last());

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var headerValues = JsonSerializer.Deserialize<IImmutableSet<string>>(content);

            // Assert
            Assert.NotNull(headerValues);
            Assert.NotEmpty(headerValues);
            Assert.Equal(listTenants.Count, headerValues.Count);
            Assert.Contains(listTenants.First(), headerValues);
            Assert.Contains(listTenants.Last(), headerValues);
        }

        [Fact]
        public async Task TenantIdValue_EmptyTenantToNoRequestHeader()
        {
            // Arrange
            var client = _server.CreateClient();

            var tenantId = string.Empty;
            client.DefaultRequestHeaders.Add(Constants.Header, tenantId);
            client.DefaultRequestHeaders.Add(Constants.Header, tenantId);

            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/mock");
            using var response = await client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var headerValues = JsonSerializer.Deserialize<IImmutableSet<string>>(content);

            // Assert
            Assert.NotNull(headerValues);
            Assert.Empty(headerValues);          
        }
    }
}