using GiG.Core.Validation.FluentValidation.MediatR.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Validation.Tests.Unit.MediatR
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddValidationPipelineBehavior_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddValidationPipelineBehavior(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}