using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Net;
using System.Net.Http;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class OrleansApplicationMetricsPrometheusTests : IAsyncLifetime
    {
        private readonly ApplicationMetricsPrometheusLifetime _applicationMetricsPrometheusLifetime;
        protected HttpClient HttpClient;

        public OrleansApplicationMetricsPrometheusTests()
        {
            _applicationMetricsPrometheusLifetime = new ApplicationMetricsPrometheusLifetime();
        }
        public async Task InitializeAsync()
        {
            await _applicationMetricsPrometheusLifetime.InitializeAsync();
            
            HttpClient = _applicationMetricsPrometheusLifetime.HttpClient;
        }

        public async Task DisposeAsync()
        {
            await _applicationMetricsPrometheusLifetime.DisposeAsync();
        }
     
        [Fact]
        public async Task ApplicationMetricsPrometheus_MetricEndpoint_StatusCodeOK()
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