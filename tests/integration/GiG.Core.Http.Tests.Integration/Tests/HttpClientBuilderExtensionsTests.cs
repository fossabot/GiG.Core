using System;
using Xunit;
using Tracing = GiG.Core.Http.DistributedTracing.Extensions;
using Tenant = GiG.Core.Http.MultiTenant.Extensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class HttpClientBuilderExtensionsTests
    {
        [Fact]
        public void AddCorrelationContextDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Tracing.HttpClientBuilderExtensions.AddCorrelationContextDelegatingHandler(null));
        }

        [Fact]
        public void AddTenantDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Tenant.HttpClientBuilderExtensions.AddTenantDelegatingHandler(null));
        }
    }
}
