using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Hosting.Tests.Integration
{
    public class ApplicationMetadataAccessorTests
    {
        private readonly TestServer _testServer;

        public ApplicationMetadataAccessorTests()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>()
                                                                   .ConfigureAppConfiguration(appConfig => { appConfig.AddJsonFile("appsettings.json"); }));
        }

        [Fact]
        public async Task ApplicationMetadataApplicationName()
        {
            //Arrange
            const string expectedResponse = "GiG.Core.Hosting.Tests.Integration";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var client = _testServer.CreateClient();

            //Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/management/name");
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedResponse, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ApplicationMetadataVersion()
        {
            //Arrange
            const string expectedResponse = "15.0.0.0";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var client = _testServer.CreateClient();

            //Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/management/version");
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedResponse, await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task ApplicationMetadataInformationalVersion()
        {
            //Arrange
            const string expectedResponse = "16.2.0";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var client = _testServer.CreateClient();

            //Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/management/version-info");
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expectedResponse, await response.Content.ReadAsStringAsync());
        }
    }
}
