﻿using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using Microsoft.Extensions.Hosting;
using Orleans;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Consul
{
    [Trait("Category", "Unit")]
    public class MembershipProviderBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureConsulClustering_MembershipProviderBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureConsulClustering(clientBuilder: null, configuration: null));
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureConsulClustering(clientBuilder: null, configurationSection: null));
            Assert.Equal("clientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureConsulClustering(siloBuilder: null, configuration: null));
            Assert.Equal("siloBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => MembershipProviderBuilderExtensions.ConfigureConsulClustering(siloBuilder: null, configurationSection: null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureConsulClustering(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
            
            exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(ctx.Configuration, x =>
                    {
                        x.ConfigureConsulClustering(configuration: null);
                    });
                }).Build());

            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().ConfigureConsulClustering(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(ctx.Configuration, x =>
                    {
                        x.ConfigureConsulClustering(configurationSection: null);
                    });
                }).Build());

            Assert.Equal("configurationSection", exception.ParamName);
        }
    }
}
