using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Http.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class HttpClientOptionsBuilderTests
    {
        [Fact]
        public void WithBaseAddress_BaseUriIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClientOptionsBuilder().WithBaseAddress(""));
            Assert.Equal("'baseUri' must not be null, empty or whitespace. (Parameter 'baseUri')", exception.Message);

            exception = Assert.Throws<ArgumentNullException>(() => new HttpClientOptionsBuilder().WithBaseAddress((Uri) null));
            Assert.Equal("baseUri", exception.ParamName);
        }

        [Fact]
        public void WithBaseAddress_RelativeUriIsNull_ThrowsArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new HttpClientOptionsBuilder().WithBaseAddress("", ""));
            Assert.Equal("'relativeUri' must not be null, empty or whitespace. (Parameter 'relativeUri')", exception.Message);
        }
    }
}
