using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureHealthChecks_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, null));
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureHealthChecks(null, configuration: null));
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x.ConfigureHealthChecks(configuration: null))));
        }

        [Fact]
        public void ConfigureHealthChecks_ConfigurationSectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x.ConfigureHealthChecks(configurationSection: null))));
        }

        [Fact]
        public void AddCachedHealthChecks_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCachedHealthChecks(null));
        }
    }
}
