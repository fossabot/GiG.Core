using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpClientBuilderTests
    {
        [Fact]
        public void AddDelegatingHandler_DelegatingHandlerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HttpClientBuilder().AddDelegatingHandler(null));
            Assert.Equal("handler", exception.ParamName);
        }

        [Fact]
        public void WithMessageHandler_MessageHandlerIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new HttpClientBuilder().WithMessageHandler(null));
            Assert.Equal("handler", exception.ParamName);
        }
    }
}
