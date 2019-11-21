using GiG.Core.Orleans.Clustering.Extensions;
using Microsoft.Extensions.Configuration;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Orleans.Clustering.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Unit.Clustering
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void UseMembershipProvider_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.UseMembershipProvider(null, null, null));
            Assert.Equal("builder", exception.ParamName);
            
            exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.UseMembershipProvider(null, configuration: null, null));
            Assert.Equal("builder", exception.ParamName);
        }
        
        [Fact]
        public void UseMembershipProvider_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().UseMembershipProvider(null, null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void UseMembershipProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().UseMembershipProvider(configuration: null, null));
            Assert.Equal("configuration", exception.ParamName);
        }
        
        [Fact]
        public void UseMembershipProvider_ConfigureProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().UseMembershipProvider(new ConfigurationBuilder().Build(), null));
            Assert.Equal("configureProvider", exception.ParamName);
        }
    }
}