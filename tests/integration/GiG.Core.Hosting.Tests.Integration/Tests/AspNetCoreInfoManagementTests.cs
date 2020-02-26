using GiG.Core.Hosting.Extensions;
using GiG.Core.Hosting.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    [CollectionDefinition("InfoManagementTests", DisableParallelization = true)]
    [Collection("InfoManagementTests")]
    public class AspNetCoreInfoManagementTests
    {
        private readonly HttpClient _httpClient;

        public AspNetCoreInfoManagementTests()
        {
            var hostBuilder = new HostBuilder()
                .UseApplicationMetadata()
                .ConfigureWebHost(webHost =>
                {
                    webHost.UseTestServer();
                    webHost.UseStartup<AspNetCoreMockStartup>();
                    webHost.ConfigureAppConfiguration(appConfig => appConfig.AddJsonFile("appsettings.json"));
                });

            _httpClient = hostBuilder.Start().GetTestClient();
        }

        [Fact]
        public async Task InfoManagement_CallEndpoint_ReturnsApplicationMetadata()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

            //Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/actuator/info");
            using var response = await _httpClient.SendAsync(request);

            var applicationMetadataDictionary = JsonSerializer.Deserialize<Dictionary<string, string>>(await response.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);            
            Assert.Equal(applicationMetadataDictionary[nameof(ApplicationMetadata.Name)], ApplicationMetadata.Name);
            Assert.Equal(applicationMetadataDictionary[nameof(ApplicationMetadata.Version)], ApplicationMetadata.Version);
            Assert.Equal(applicationMetadataDictionary[nameof(ApplicationMetadata.InformationalVersion)], ApplicationMetadata.InformationalVersion);
            Assert.Equal(applicationMetadataDictionary[nameof(ApplicationMetadata.Checksum)], ApplicationMetadata.Checksum);
        }
    }
}