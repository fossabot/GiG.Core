using GiG.Core.Http.Extensions;
using System;
using System.Net.Http;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
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
    }
}
