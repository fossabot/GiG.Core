using Bogus;
using GiG.Core.Orleans.Clustering.Kubernetes.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using k8s;
using Orleans;
using Orleans.Runtime;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Orleans.Tests.Integration.Helpers
{
    public abstract class AbstractKubernetesTests
    {
        protected string SiloName { get; set; }
        protected IClusterClient ClusterClient { get; set; }
        protected KubernetesSiloOptions KubernetesSiloOptions { get; set; }

        [Fact]
        public async Task GetValueAsync_CallGrain_ReturnsExpectedInteger()
        {
            //Arrange
            var grain = ClusterClient.GetGrain<IEchoTestGrain>(Guid.NewGuid().ToString());
            var expectedValue = new Randomizer().Int();
            await grain.SetValueAsync(expectedValue);

            //Act 
            var actualValue = await grain.GetValueAsync();

            //Assert
            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task GetSiloMembership_Kubernetes_ReturnsSiloInformation()
        {
            //Arrange
            var expectedSiloName = SiloName;

            //Act
            var config = KubernetesClientConfiguration.BuildDefaultConfig();
            config.AccessToken = KubernetesSiloOptions.ApiToken;

            using var client = new Kubernetes(config);
            client.BaseUri = new Uri(KubernetesSiloOptions.ApiEndpoint.Remove(KubernetesSiloOptions.ApiEndpoint.LastIndexOf("/"), 4));
            var membershipTable = await client.GetNamespacedCustomObjectAsync(KubernetesSiloOptions.Group, "v1", "orleanstest", "silos", string.Empty);

            JsonDocument.Parse(membershipTable.ToString()).RootElement.TryGetProperty("items", out var siloElements);
            var siloElement = siloElements.EnumerateArray().ToList().FirstOrDefault(x => x.GetProperty("siloName").ToString() == SiloName);

            //Assert
            Assert.Equal(SiloStatus.Active.ToString(), siloElement.GetProperty("status").ToString(), true);
        }
    }
}
