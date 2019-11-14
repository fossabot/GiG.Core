﻿using GiG.Core.Http.Security.Hmac.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;
using HttpClientBuilderExtensions = GiG.Core.Http.Security.Hmac.Extensions.HttpClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit.Security
{
    [Trait("Category", "Unit")]
    public class HttpClientBuilderExtensionsTests
    {
        [Fact]
        public void AddHmacDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.AddHmacDelegatingHandler(null));
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.ConfigureDefaultHmacDelegatingHandlerOptionProvider(null, configuration: null));
            Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.ConfigureDefaultHmacDelegatingHandlerOptionProvider(null, configurationSection: null));
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                    new ServiceCollection().AddHttpClient("Default").ConfigureDefaultHmacDelegatingHandlerOptionProvider(configuration: null));
        }

        [Fact]
        public void ConfigureDefaultHmacDelegatingHandlerOptionProvider_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                    new ServiceCollection().AddHttpClient("Default").ConfigureDefaultHmacDelegatingHandlerOptionProvider(configurationSection: null));
        }
    }
}
