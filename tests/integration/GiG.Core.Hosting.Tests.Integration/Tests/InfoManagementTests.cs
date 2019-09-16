using GiG.Core.Hosting.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Hosting.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class InfoManagementTests
    {
        private readonly TestServer _testServer;

        public InfoManagementTests()
        {
            _testServer = new TestServer(new WebHostBuilder().UseStartup<MockStartup>()
                .ConfigureAppConfiguration(appConfig => { appConfig.AddJsonFile("appsettings.json"); }));
        }

        [Fact]
        public async Task InfoManagementShouldReturnInfo()
        {
            //Arrange
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            var client = _testServer.CreateClient();

            //Act
            using var request = new HttpRequestMessage(HttpMethod.Get, "/actuator/info");
            using var response = await client.SendAsync(request);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}