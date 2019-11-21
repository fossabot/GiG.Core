using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMembershipProviderConsulTests : AbstractConsulTests, IClassFixture<ConsulMembershipProviderFixture>
    {
        public OrleansMembershipProviderConsulTests(ConsulMembershipProviderFixture consulClusterFixture)
        {
            SiloName = consulClusterFixture.SiloName;
            ClusterClient = consulClusterFixture.ClusterClient;
            HttpClientFactory = consulClusterFixture.HttpClientFactory;

            var options = consulClusterFixture.ConsulOptions.Value;
            ConsulKvStoreBaseAddress = $"{options.Address}/v1/kv/{options.KvRootFolder}/";
        }
    }
}