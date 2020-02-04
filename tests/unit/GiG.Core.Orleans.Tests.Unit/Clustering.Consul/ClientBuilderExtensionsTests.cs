using GiG.Core.Orleans.Clustering.Consul.Extensions;
using Orleans;
using System;
using System.Configuration;
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
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.ConfigureConsulClustering(null, configuration: null));
            Assert.Equal("clientBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ClientBuilder().ConfigureConsulClustering(null));
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureConsulClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}