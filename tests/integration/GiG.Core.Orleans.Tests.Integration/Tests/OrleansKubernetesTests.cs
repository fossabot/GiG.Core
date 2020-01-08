using System.Threading.Tasks;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using GiG.Core.Orleans.Tests.Integration.Lifetimes;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansKubernetesTests : AbstractKubernetesTests, IAsyncLifetime
    {
        private readonly KubernetesClusterLifetime _kubernetesClusterLifetime;

        public OrleansKubernetesTests()
        {
            _kubernetesClusterLifetime = new KubernetesClusterLifetime();
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