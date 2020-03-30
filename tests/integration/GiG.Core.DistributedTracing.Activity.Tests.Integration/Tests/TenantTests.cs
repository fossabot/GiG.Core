using Bogus;
using GiG.Core.DistributedTracing.Activity.Tests.Integration.Lifetimes;
using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class TenantTests : IClassFixture<MockWebFixture>
    {
        private readonly MockWebFixture _fixture;
   
        public TenantTests(MockWebFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async void ActivityAccessor_NoTenantIdHeaderSet_ShouldReturn()
        {
            //Arrange
            var testEndpointUrl = $@"{WebFixture.BaseUrl}/api/mock/tenants";
            using var client = _fixture.HttpClient;
            
            //Act
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            var tenantIds = JsonSerializer.Deserialize<List<string>>(responseBody);

            //Assert
            Assert.Empty(tenantIds);
        }

        [Fact]
        public async void ActivityAccessor_SingleTenantIdHeaderSet_ShouldSetTenantIdBaggage()
        {
            //Arrange
            var expectedTenant = new Faker().Random.String2(10);
            
            var testEndpointUrl = $@"{WebFixture.BaseUrl}/api/mock/tenants";
            using var client = _fixture.HttpClient;
            client.DefaultRequestHeaders.Add(Constants.Header, expectedTenant);
            
            //Act
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            var tenantIds = JsonSerializer.Deserialize<List<string>>(responseBody);

            //Assert
            Assert.Single(tenantIds);
            Assert.Equal(expectedTenant, tenantIds.First());
        }

        [Fact]
        public async void ActivityAccessor_MultipleTenantIdHeadersSet_ShouldSetTenantIdsInBaggage()
        {
            //Arrange
            var expectedTenant1 = new Faker().Random.String2(10);
            var expectedTenant2 = new Faker().Random.String2(10);
            
            var testEndpointUrl = $@"{WebFixture.BaseUrl}/api/mock/tenants";
            using var client = _fixture.HttpClient;
            client.DefaultRequestHeaders.Add(Constants.Header, expectedTenant1);
            client.DefaultRequestHeaders.Add(Constants.Header, expectedTenant2);
            
            //Act
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            var tenantIds = JsonSerializer.Deserialize<List<string>>(responseBody);

            //Assert
            Assert.Equal(2, tenantIds.Count);
            Assert.Contains(expectedTenant1, tenantIds);
            Assert.Contains(expectedTenant2, tenantIds);
        }
    }
}