using GiG.Core.Http.MultiTenant.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit.MultiTenant
{
    [Trait("Category", "Unit")]
    public class HttpClientBuilderExtensionsTests
    {
        [Fact]
        public void AddTenantDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.AddTenantDelegatingHandler(null));
        }
    }
}
