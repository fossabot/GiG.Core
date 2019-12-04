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
            var exception = Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddReadyCheck<CachedUnHealthyCheck>(null, ""));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddReadyCheck(null, "", null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddReadyCheck_HealthCheckNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ServiceCollection()
                .AddCachedHealthChecks()
                .AddReadyCheck<CachedUnHealthyCheck>(null));
            Assert.Equal("'name' must not be null, empty or whitespace. (Parameter 'name')", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => new ServiceCollection()
                .AddCachedHealthChecks()
                .AddReadyCheck(null, null));
            Assert.Equal("'name' must not be null, empty or whitespace. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void AddReadyCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddReadyCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy));
            Assert.Equal("instance", exception.ParamName);
        }

        [Fact]
        public void AddLiveCheck_HealthChecksBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddLiveCheck<CachedUnHealthyCheck>(null, ""));
            Assert.Equal("builder", exception.ParamName);

            exception = Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddLiveCheck(null, "", null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddLiveCheck_HealthCheckNameIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck<CachedUnHealthyCheck>(null));
            Assert.Equal("'name' must not be null, empty or whitespace. (Parameter 'name')", exception.Message);

            exception = Assert.Throws<ArgumentException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck(null, null));
            Assert.Equal("'name' must not be null, empty or whitespace. (Parameter 'name')", exception.Message);
        }

        [Fact]
        public void AddLiveCheck_InstanceIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ServiceCollection()
                        .AddCachedHealthChecks()
                        .AddLiveCheck(nameof(CachedUnHealthyCheck), null, HealthStatus.Unhealthy));
            Assert.Equal("instance", exception.ParamName);
        }
    }
}
