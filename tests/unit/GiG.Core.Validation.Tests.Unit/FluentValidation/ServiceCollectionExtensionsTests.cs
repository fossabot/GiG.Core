using GiG.Core.Validation.FluentValidation.Web.Extensions;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Validation.Tests.Unit.FluentValidation
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ConfigureApiBehaviorOptions_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.ConfigureApiBehaviorOptions(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}