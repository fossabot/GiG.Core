using GiG.Core.MultiTenant.Web.Extensions;
using System;
using Xunit;

namespace GiG.Core.MultiTenant.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseTenantIdMiddleware_BuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UseTenantIdMiddleware(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}