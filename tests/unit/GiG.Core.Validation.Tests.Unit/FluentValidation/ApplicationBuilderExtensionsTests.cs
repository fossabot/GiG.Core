using GiG.Core.Validation.FluentValidation.Web.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Validation.Tests.Unit.FluentValidation
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