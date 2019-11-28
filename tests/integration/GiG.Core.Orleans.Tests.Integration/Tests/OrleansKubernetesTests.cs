using GiG.Core.Orleans.Tests.Integration.Fixtures;
using GiG.Core.Orleans.Tests.Integration.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "IntegrationWithDependency")]
    public class OrleansKubernetesTests : AbstractKubernetesTests, IClassFixture<KubernetesClusterFixture>
    {
        public OrleansKubernetesTests(KubernetesClusterFixture kubernetesClusterFixture)
        {
            SiloName = kubernetesClusterFixture.SiloName;
            ClusterClient = kubernetesClusterFixture.ClusterClient;
            KubernetesSiloOptions = kubernetesClusterFixture.KubernetesOptions.Value;
        }
    }
}