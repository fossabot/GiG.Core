using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMembershipProviderKubernetesTests : AbstractKubernetesTests, IClassFixture<KubernetesMembershipProviderFixture>
    {
        public OrleansMembershipProviderKubernetesTests(KubernetesMembershipProviderFixture kubernetesClusterFixture)
        {
            SiloName = kubernetesClusterFixture.SiloName;
            ClusterClient = kubernetesClusterFixture.ClusterClient;
            KubernetesSiloOptions = kubernetesClusterFixture.KubernetesOptions.Value;
        }
    }
}