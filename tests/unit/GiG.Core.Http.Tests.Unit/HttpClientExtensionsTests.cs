using GiG.Core.Http.Extensions;
using Microsoft.Extensions.Configuration;
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
            var exception = Assert.Throws<ArgumentNullException>(() => HttpClientExtensions.FromConfiguration(null, null, ""));
            Assert.Equal("client", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => HttpClientExtensions.FromConfiguration(null, "", null));
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public void FromConfiguration_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HttpClient().FromConfiguration(null, ""));
            Assert.Equal("configuration", exception.ParamName);
        }

        [Fact]
        public void FromConfiguration_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HttpClient().FromConfiguration("", null));
            Assert.Equal("configurationSection", exception.ParamName);
        }

        [Fact]
        public void FromConfiguration_BaseUriIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(null, new ConfigurationBuilder().Build().GetSection("HttpClient")));
            Assert.Equal("Missing baseUri.", exception.Message);
        }

        [Fact]
        public void FromConfiguration_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(new ConfigurationBuilder().Build(), null));
            Assert.Equal("Missing sectionName.", exception.Message);
        }
    }
}