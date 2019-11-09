using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Integration.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HealthChecksBuilderExtensionsTests
    {
        [Fact]
        public void AddReadyCheck_HealthChecksBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddReadyCheck<CachedUnHealthyCheck>(null, ""));
            Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddReadyCheck(null, "", null));
        }

        [Fact]
        public void AddReadyCheck_HealthCheckNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddReadyCheck<CachedUnHealthyCheck>(null))));

            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddReadyCheck(null, null))));
        }

        [Fact]
        public void AddReadyCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddReadyCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy))));
        }

        [Fact]
        public void AddLiveCheck_HealthChecksBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddLiveCheck<CachedUnHealthyCheck>(null, ""));
            Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddLiveCheck(null, "", null));
        }

        [Fact]
        public void AddLiveCheck_HealthCheckNameIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddLiveCheck<CachedUnHealthyCheck>(null))));

            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddLiveCheck(null, null))));
        }

        [Fact]
        public void AddLiveCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TestServer(
                new WebHostBuilder().UseStartup<MockStartupWithDefaultConfiguration>()
                    .ConfigureServices(x => x
                        .AddCachedHealthChecks()
                        .AddLiveCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy))));
        }
    }
}
