using GiG.Core.Http.Extensions;
using GiG.Core.Http.Tests.Unit.Mocks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Net.Http;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpClientExtensionsTests
    {
        [Fact]
        public void FromConfiguration_HttpClientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientExtensions.FromConfiguration(null, null, ""));
            Assert.Throws<ArgumentNullException>(() => HttpClientExtensions.FromConfiguration(null, "", null));
        }

        [Fact]
        public void FromConfiguration_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpClient().FromConfiguration(null, ""));
        }

        [Fact]
        public void FromConfiguration_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpClient().FromConfiguration("", null));
        }

        [Fact]
        public void FromConfiguration_BaseUriIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentException>(() => new ServiceCollection().AddRefitClient<IMockRestClient>()
                .ConfigureHttpClient(c => c.FromConfiguration(null, new ConfigurationBuilder().Build().GetSection(""))));
        }

        [Fact]
        public void FromConfiguration_SectionNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentException>(() => new ServiceCollection().AddRefitClient<IMockRestClient>()
                .ConfigureHttpClient(c => c.FromConfiguration(new ConfigurationBuilder().Build(), null)));
        }
    }
}