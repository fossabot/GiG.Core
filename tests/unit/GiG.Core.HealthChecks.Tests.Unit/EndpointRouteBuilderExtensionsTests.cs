using GiG.Core.HealthChecks.AspNetCore.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.HealthChecks.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class EndpointRouteBuilderExtensionsTests
    {
        [Fact]
        public void MapHealthChecks_EndpointRouterBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EndpointRouteBuilderExtensions.MapHealthChecks(null));
            Assert.Equal("endpointRouteBuilder", exception.ParamName);
        }
    }
}
