using GiG.Core.Web.FluentValidation.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Web.Tests.Unit.FluentValidation
{
    [Trait("Category", "Unit")]
    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseFluentValidationMiddleware_ApplicationBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ApplicationBuilderExtensions.UseFluentValidationMiddleware(null));
            Assert.Equal("builder", exception.ParamName);
        }
    }
}