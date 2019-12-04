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
            Assert.Equal("'name' must not be null, empty or whitespace. (Parameter 'name')", exception.Message);
        }
    }
}
