using GiG.Core.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Net.Http;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpClientExtensionsTests
    {
        private readonly IConfiguration _configuration;

        public HttpClientExtensionsTests()
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true)
                .Build();
        }

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
        public void FromConfiguration_ConfigurationSectionIsNull_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new HttpClient().FromConfiguration("", null));
            Assert.Equal("Configuration Section '' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromConfiguration_BaseUriIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(null, _configuration.GetSection("HttpClient")));
            Assert.Equal("Missing baseUri.", exception.Message);
        }

        [Fact]
        public void FromConfiguration_IncorrectConfigurationSectionName_ThrowsConfigurationErrorsException()
        {
            var exception = Assert.Throws<ConfigurationErrorsException>(() => new HttpClient().FromConfiguration("/api", _configuration.GetSection("Http")));
            Assert.Equal("Configuration Section 'Http' is incorrect.", exception.Message);
        }

        [Fact]
        public void FromConfiguration_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClient().FromConfiguration(_configuration, null));
            Assert.Equal("Missing sectionName.", exception.Message);
        }
    }
}