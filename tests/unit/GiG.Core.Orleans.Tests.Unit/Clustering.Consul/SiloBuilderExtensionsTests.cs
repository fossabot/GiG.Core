using GiG.Core.Orleans.Clustering.Consul.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using Xunit;
using SiloBuilderExtensions = GiG.Core.Orleans.Clustering.Consul.Extensions.SiloBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Consul
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureConsulClustering_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureConsulClustering(null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureConsulClustering(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureConsulClustering(configuration: null);
                }).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureConsulClustering(null);
                }).Build());
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }
    }
}
