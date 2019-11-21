using GiG.Core.Http.Security.Hmac.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit.Security
{
    [Trait("Category", "Unit")]
    public class HttpRequestMessageExtensionsTests
    {
        [Fact]
        public void GetBodyAsync_HttpRequestMessageIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.ThrowsAsync<ArgumentNullException>(() => HttpRequestMessageExtensions.GetBodyAsync(null));
            Assert.Equal("httpRequest", exception.Result.ParamName);
        }
    }
}
