using GiG.Core.Orleans.Clustering.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
using SiloBuilderExtensions = GiG.Core.Orleans.Clustering.Extensions.SiloBuilderExtensions;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void UseMembershipProvider_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.UseMembershipProvider(null, null, null));
            Assert.Equal("builder", exception.ParamName);
            
            exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.UseMembershipProvider(null, configuration: null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void UseMembershipProvider_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(null, null);
                }).Build());

            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void UseMembershipProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(configuration: null, null);
                }).Build());

            Assert.Equal("configuration", exception.ParamName);
        }
        
        [Fact]
        public void UseMembershipProvider_ConfigureProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Host.CreateDefaultBuilder()
                .UseOrleans((ctx, sb) =>
                {
                    sb.UseMembershipProvider(ctx.Configuration, null);
                }).Build());

            Assert.Equal("configureProvider", exception.ParamName);
        }
    }
}
