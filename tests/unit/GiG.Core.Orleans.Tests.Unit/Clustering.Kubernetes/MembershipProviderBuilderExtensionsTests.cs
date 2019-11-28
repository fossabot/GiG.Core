using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using System.Configuration;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Kubernetes
{
    [Trait("Category", "Unit")]
    public class MembershipProviderBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureKubernetesClustering_MembershipProviderBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureKubernetesClustering(clientBuilder: null, configuration: null));
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureKubernetesClustering(clientBuilder: null, configurationSection: null));
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureKubernetesClustering(siloBuilder: null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureKubernetesClustering(siloBuilder: null, configurationSection: null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureKubernetesClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
            
            exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(ctx.Configuration, x =>
                    {
                        x.ConfigureKubernetesClustering(configuration: null);
                    });
                }).Build());

            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureKubernetesClustering_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ClientBuilder().ConfigureKubernetesClustering(configurationSection: null));
            Assert.Equal("Configuration section '' is incorrect.", exception.Message);

            exception = Assert.Throws<ConfigurationErrorsException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(ctx.Configuration, x =>
                    {
                        x.ConfigureKubernetesClustering(configurationSection: null);
                    });
                }).Build());

            Assert.Equal("Configuration section '' is incorrect.", exception.Message);
        }
    }
}
