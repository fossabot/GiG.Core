using GiG.Core.Messaging.MassTransit.Extensions;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.MassTransit
{
    [Trait("Category", "Unit")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddMassTransitContext_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.AddMassTransitContext(null));
            Assert.Equal("services", exception.ParamName);
        }
    }
}
