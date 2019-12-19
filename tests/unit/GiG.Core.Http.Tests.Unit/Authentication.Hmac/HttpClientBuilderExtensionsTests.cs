using GiG.Core.Http.Authentication.Hmac.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using Xunit;
using HttpClientBuilderExtensions = GiG.Core.Http.Authentication.Hmac.Extensions.HttpClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit.Security
{
    [Trait("Category", "Unit")]
    public class HttpClientBuilderExtensionsTests
    {
        [Fact]
        public void AddHmacDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.AddHmacDelegatingHandler(null));
            Assert.Equal("httpClientBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.ConfigureDefaultHmacDelegatingHandlerOptionProvider(null, configuration: null));
            Assert.Equal("httpClientBuilder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.ConfigureDefaultHmacDelegatingHandlerOptionProvider(null, configurationSection: null));
            Assert.Equal("httpClientBuilder", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                    new ServiceCollection().AddHttpClient("Default").ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration: null));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() =>
                    new ServiceCollection().AddHttpClient("Default").ConfigureDefaultHmacDelegatingHandlerOptionProvider(configurationSection: null));
            Assert.Equal("Configuration Section '' is incorrect.", exception.Message);
        }
    }
}
