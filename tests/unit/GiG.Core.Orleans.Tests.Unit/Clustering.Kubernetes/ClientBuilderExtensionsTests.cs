using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Orleans.Clustering.Kubernetes.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Kubernetes
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureKubernetesClustering_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureKubernetesClustering(null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureKubernetesClustering(null, configurationSection: null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureKubernetesClustering(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureKubernetesClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}