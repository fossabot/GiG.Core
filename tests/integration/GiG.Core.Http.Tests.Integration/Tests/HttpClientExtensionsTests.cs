using GiG.Core.Http.Extensions;
using GiG.Core.Http.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;
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

        [Fact]
        public void FromConfiguration_BaseUriIsNull_ThrowsArgumentNullException()
        {
            var host = Host.CreateDefaultBuilder()
                   .ConfigureServices((x, y) =>
                   {
                       y.AddRefitClient<IMockRestClient>()
                           .ConfigureHttpClient(c => c.FromConfiguration(null, x.Configuration.GetSection("")));
                   }).Build();

            Assert.Throws<ArgumentNullException>(() => host.Services.GetRequiredService<IMockRestClient>());
        }

        [Fact]
        public void FromConfiguration_SectionNameIsNull_ThrowsArgumentNullException()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((x, y) =>
                {
                    y.AddRefitClient<IMockRestClient>()
                        .ConfigureHttpClient(c => c.FromConfiguration(x.Configuration, null));
                }).Build();

            Assert.Throws<ArgumentNullException>(() => host.Services.GetRequiredService<IMockRestClient>());        }
    }
}
