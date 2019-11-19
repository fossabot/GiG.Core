using GiG.Core.DistributedTracing.MassTransit.Extensions;
using System;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit.MassTransit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCorrelationAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddCorrelationAccessor(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
