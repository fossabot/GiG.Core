using GiG.Core.Web.Hosting.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Hosting
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureForwardedHeaders_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureForwardedHeaders(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}