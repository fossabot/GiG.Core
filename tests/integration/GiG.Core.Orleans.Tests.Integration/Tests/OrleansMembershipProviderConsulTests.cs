using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMembershipProviderConsulTests : AbstractConsulTests, IClassFixture<MembershipProviderFixture>
    {
        public OrleansMembershipProviderConsulTests(MembershipProviderFixture consulClusterFixture)
        {
            _siloName = consulClusterFixture.SiloName;
            _clusterClient = consulClusterFixture.ClusterClient;
            _httpClientFactory = consulClusterFixture.HttpClientFactory;

            var options = consulClusterFixture.ConsulOptions.Value;
            _consulKVStoreBaseAddress = $"{options.Address}/v1/kv/{options.KvRootFolder}/";
        }
    }
}