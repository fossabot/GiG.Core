using GiG.Core.Web.Authentication.Hmac.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Authentication.Hmac
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddHmacAuthentication_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddHmacAuthentication(null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacOptionProvider_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureDefaultHmacOptionProvider(null, null));
            Assert.Equal("services", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureDefaultHmacOptionProvider(null, configuration: null));
            Assert.Equal("services", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacOptionProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection().ConfigureDefaultHmacOptionProvider(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacOptionProvider_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new ServiceCollection().ConfigureDefaultHmacOptionProvider(null));
            Assert.Equal("Configuration Section '' is incorrect.", exception.Message);
        }
    }
}