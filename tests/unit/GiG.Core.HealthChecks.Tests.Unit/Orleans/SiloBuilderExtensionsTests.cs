using GiG.Core.HealthChecks.Orleans.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class SiloBuilderExtensionsTests
    {
        [Fact]
        public void AddHealthCheckDependencies_SiloBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => SiloBuilderExtensions.AddHealthCheckDependencies(null));
            Assert.Equal("siloBuilder", exception.ParamName);
        }
    }
}
