using GiG.Core.Orleans.Clustering.Consul.Extensions;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Orleans.Clustering.Consul.Extensions.ClientBuilderExtensions;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Consul
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureConsulClustering_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureConsulClustering(null, null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureConsulClustering(null, configuration: null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureConsulClustering(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureConsulClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}