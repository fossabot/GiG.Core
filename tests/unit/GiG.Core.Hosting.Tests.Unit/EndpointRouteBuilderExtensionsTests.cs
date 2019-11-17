using GiG.Core.Hosting.AspNetCore.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Hosting.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class EndpointRouteBuilderExtensionsTests
    {
        [Fact]
        public void MapInfoManagement_EndpointRouteBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => EndpointRouteBuilderExtensions.MapInfoManagement(null));
            Assert.Equal("endpointRouteBuilder", exception.ParamName);
        }
    }
}
