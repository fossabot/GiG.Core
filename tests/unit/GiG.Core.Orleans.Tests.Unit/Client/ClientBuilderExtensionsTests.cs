using GiG.Core.Orleans.Client.Extensions;
using Microsoft.Extensions.Configuration;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Orleans.Client.Extensions.ClientBuilderExtensions;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Client
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void BuildAndConnect_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.BuildAndConnect(null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureCluster(null, null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureCluster(null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureCluster(null, "", null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureCluster(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureCluster(configuration: null));
            Assert.Equal("configuration", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureCluster(configuration: null, clusterName: ""));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureCluster_ClusterNameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ClientBuilder().ConfigureCluster("", new ConfigurationBuilder().Build()));
            Assert.Equal("Missing clusterName.", exception.Message);
        }

        [Fact]
        public void ConfigureAssemblies_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureAssemblies(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddAssemblies(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureAssemblies_AssembliesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureAssemblies(null));
            Assert.Equal("assemblies", exception.ParamName);
        }

        [Fact]
        public void AddAssemblies_TypesIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().AddAssemblies(null));
            Assert.Equal("types", exception.ParamName);
        }
    }
}