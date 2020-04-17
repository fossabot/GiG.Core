using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMetricsPrometheusTests : AbstractMetricsPrometheusTests, IAsyncLifetime
    {
        private readonly MetricsPrometheusLifetime _metricsPrometheusLifetime;

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
     
    }
}