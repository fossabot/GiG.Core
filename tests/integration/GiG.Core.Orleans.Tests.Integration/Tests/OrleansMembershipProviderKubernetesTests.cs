using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansMembershipProviderKubernetesTests : AbstractKubernetesTests, IAsyncLifetime
    {
        private readonly KubernetesMembershipProviderLifetime _kubernetesClusterLifetime;

        public OrleansMembershipProviderKubernetesTests()
        {
            _kubernetesClusterLifetime = new KubernetesMembershipProviderLifetime();
        }

        public async Task InitializeAsync()
        {
            await _kubernetesClusterLifetime.InitializeAsync();

            SiloName = _kubernetesClusterLifetime.SiloName;
            ClusterClient = _kubernetesClusterLifetime.ClusterClient;
            KubernetesSiloOptions = _kubernetesClusterLifetime.KubernetesOptions.Value;
        }

        public async Task DisposeAsync()
        {
            await _kubernetesClusterLifetime.DisposeAsync();
        }
    }
}