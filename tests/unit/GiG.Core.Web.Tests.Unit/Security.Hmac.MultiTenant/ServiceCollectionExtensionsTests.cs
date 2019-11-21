using GiG.Core.Web.Security.Hmac.MultiTenant.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Security.Hmac.MultiTenant
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureMultiTenantHmacOptionProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureMultiTenantHmacOptionProvider(null, configuration: null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureMultiTenantHmacOptionProvider(null, configurationSection: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureMultiTenantHmacOptionProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureMultiTenantHmacOptionProvider(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureMultiTenantHmacOptionProvider_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureMultiTenantHmacOptionProvider(configurationSection: null));
            Assert.Equal("configurationSection", exception.ParamName);
        }
    }
}