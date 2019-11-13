using GiG.Core.HealthChecks.Extensions;
using GiG.Core.HealthChecks.Tests.Unit.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit
{
    [Trait("Category", "Unit")]
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
            Assert.Throws<ArgumentException>(() => new ServiceCollection()
                .AddCachedHealthChecks()
                .AddReadyCheck<CachedUnHealthyCheck>(null));

            Assert.Throws<ArgumentException>(() => new ServiceCollection()
                .AddCachedHealthChecks()
                .AddReadyCheck(null, null));
        }

        [Fact]
        public void AddReadyCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddReadyCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy));
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
            Assert.Throws<ArgumentException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck<CachedUnHealthyCheck>(null));

            Assert.Throws<ArgumentException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck(null, null));
        }

        [Fact]
        public void AddLiveCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy));
        }
    }
}
