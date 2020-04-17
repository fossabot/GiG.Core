using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Helpers
{
    public abstract class AbstractMetricsPrometheusTests
    {
        protected HttpClient HttpClient;

        [Fact]
        public async Task MetricsPrometheus_MetricEndpoint_StatusCodeOK()
        {
            //Arrange
            var metricsEndpoint = "metrics";

            //Act 
            var response = await HttpClient.GetAsync(metricsEndpoint);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
