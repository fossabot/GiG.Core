using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Context.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddRequestContextAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Web.Extensions.ServiceCollectionExtensions.AddRequestContextAccessor(null));
            Assert.Throws<ArgumentNullException>(() => Orleans.Extensions.ServiceCollectionExtensions.AddRequestContextAccessor(null));
        }
    }
}
