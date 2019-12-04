using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansConsulTests : AbstractConsulTests, IAsyncLifetime
    {
        private readonly ConsulClusterLifetime _consulLifetime;

        public OrleansConsulTests()
        {
            _consulLifetime = new ConsulClusterLifetime();
        }

        public async Task InitializeAsync()
        {
            await _consulLifetime.InitializeAsync();

            SiloName = _consulLifetime.SiloName;
            ClusterClient = _consulLifetime.ClusterClient;
            HttpClientFactory = _consulLifetime.HttpClientFactory;
            ConsulKvStoreBaseAddress = _consulLifetime.ConsulKvStoreBaseAddress;
        }

        public async Task DisposeAsync()
        {
            await _consulLifetime.DisposeAsync();
        }
    }       
}