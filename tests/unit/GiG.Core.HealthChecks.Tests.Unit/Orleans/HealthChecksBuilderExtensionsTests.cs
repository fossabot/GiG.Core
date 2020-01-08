using GiG.Core.HealthChecks.Orleans.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class HealthChecksBuilderExtensionsTests
    {
        [Fact]
        public void AddOrleansHealthCheck_HealthCheckBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => HealthChecksBuilderExtensions.AddOrleansHealthCheck(null));
            Assert.Equal("healthChecksBuilder", exception.ParamName);
        }
    }
}
