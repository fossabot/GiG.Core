using Bogus;
using GiG.Core.DistributedTracing.Activity.Tests.Integration.Lifetimes;
using GiG.Core.MultiTenant.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.DistributedTracing.Activity.Tests.Integration.Tests
{
    public class TenantTests : IAsyncLifetime
    {
        private readonly ActivityLifetime _activityLifetime;
   
        public TenantTests()
        {
            _activityLifetime = new ActivityLifetime();
        }

        public async Task InitializeAsync()
        {
            await _activityLifetime.InitializeAsync();
        } 

        public async Task DisposeAsync()
        {
            await _activityLifetime.DisposeAsync();
        }

        [Fact]
        public async void ActivityAccessor_TenantIdPassedToBaggage_ShouldSetTenantId()
        {
            //Arrange
            var expectedTenant = new Faker().Random.String2(10);
            
            var testEndpointUrl = $@"{ActivityLifetime.BaseUrl}/api/mock/tenants";
            using var client = _activityLifetime.HttpClientFactory.CreateClient("tenants");
            client.DefaultRequestHeaders.Add(Constants.Header, expectedTenant);
            
            //Act
            var response = await client.GetAsync(testEndpointUrl);
            var responseBody = await response.Content.ReadAsStringAsync();

            var tenantIds = JsonSerializer.Deserialize<List<string>>(responseBody);

            //Assert
            Assert.Single(tenantIds);
            Assert.Equal(expectedTenant, tenantIds.First());
        }
    }
}