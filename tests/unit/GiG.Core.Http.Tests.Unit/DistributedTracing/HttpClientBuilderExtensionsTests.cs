using GiG.Core.Http.DistributedTracing.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit.DistributedTracing
{
    [Trait("Category", "Unit")]
    public class HttpClientBuilderExtensionsTests
    {
        [Fact]
        public void AddCorrelationContextDelegatingHandler_HttpClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => HttpClientBuilderExtensions.AddCorrelationContextDelegatingHandler(null));
        }
    }
}
