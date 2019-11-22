using GiG.Core.Web.Docs.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.Docs
{
    [Trait("Category", "Unit")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseApiDocs_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UseApiDocs(null));
            Assert.Equal("app", exception.ParamName);
        }
    }
}
