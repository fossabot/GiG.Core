using GiG.Core.HealthChecks.Abstractions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Helpers
{
    public abstract class AbstractHealthCheckTests
    {
        protected IHttpClientFactory HttpClientFactory;
        protected HealthChecksOptions HealthChecksOptions;
        protected int Port;
       
        [Fact]
        public async Task LiveHealthCheck_ReturnsHealthyStatus()
        {
            //Arrange
            var healthCheckEndpointUrl = @$"http://localhost:{Port}{HealthChecksOptions.LiveUrl}";

            //Act 
            var response = await HttpClientFactory.CreateClient().GetAsync(healthCheckEndpointUrl);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ReadyHealthCheck_ReturnsHealthyStatus()
        {
            //Arrange
            var healthCheckEndpointUrl = @$"http://localhost:{Port}{HealthChecksOptions.ReadyUrl}";

            //Act 
            var response = await HttpClientFactory.CreateClient().GetAsync(healthCheckEndpointUrl);

            //Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
