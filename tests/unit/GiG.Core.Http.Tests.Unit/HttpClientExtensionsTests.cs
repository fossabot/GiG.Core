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
            Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(null, new ConfigurationBuilder().Build().GetSection("HttpClient")));
        }

        [Fact]
        public void FromConfiguration_SectionNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(new ConfigurationBuilder().Build(), null));
        }
    }
}