﻿using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering.Kubernetes
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureConsulClustering_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureKubernetesClustering(null, configuration: null));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.ConfigureKubernetesClustering(null, configurationSection: null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureKubernetesClustering(configuration: null);
                }).Build());
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureConsulClustering_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.ConfigureKubernetesClustering(configurationSection: null);
                }).Build());
            Assert.Equal("configurationSection", exception.ParamName);
        }
    }
}
