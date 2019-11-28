using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using Orleans;
using System;
using System.Configuration;
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
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureKubernetesClustering(null, configurationSection: null));
            Assert.Equal("clientBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ClientBuilder().ConfigureKubernetesClustering(configurationSection: null));
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureKubernetesClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}