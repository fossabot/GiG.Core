using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMembershipProviderConsulTests : AbstractConsulTests, IAsyncLifetime
    {
        private readonly ConsulMembershipProviderLifetime _consulMembershipLifetime;

        public OrleansMembershipProviderConsulTests()
        {
            _consulMembershipLifetime = new ConsulMembershipProviderLifetime();
        }

        public async Task InitializeAsync()
        {
            await _consulMembershipLifetime.InitializeAsync();

            SiloName = _consulMembershipLifetime.SiloName;
            ClusterClient = _consulMembershipLifetime.ClusterClient;
            HttpClientFactory = _consulMembershipLifetime.HttpClientFactory;
            ConsulKvStoreBaseAddress = _consulMembershipLifetime.ConsulKvStoreBaseAddress;
        }

        public async Task DisposeAsync()
        {
            await _consulMembershipLifetime.DisposeAsync();
        }
    }
}