using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Net;
using System.Net.Http;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMetricsPrometheusTests : IAsyncLifetime
    {
        private readonly MetricsPrometheusLifetime _metricsPrometheusLifetime;
        protected HttpClient HttpClient;

        public OrleansMetricsPrometheusTests()
        {
            _metricsPrometheusLifetime = new MetricsPrometheusLifetime();
        }
        public async Task InitializeAsync()
        {
            await _metricsPrometheusLifetime.InitializeAsync();
            
            HttpClient = _metricsPrometheusLifetime.HttpClient;
        }

        public async Task DisposeAsync()
        {
            await _metricsPrometheusLifetime.DisposeAsync();
        }
     
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