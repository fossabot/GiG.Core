using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpClientFactoryTests
    {
        [Fact]
        public void GetOrAdd_NameIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => HttpClientFactory.GetOrAdd(""));
            Assert.Equal("Missing name.", exception.Message);
        }
    }
}
