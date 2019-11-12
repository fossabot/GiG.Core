using System;
using Xunit;
using CorrelationServiceCollectionExtensions = GiG.Core.DistributedTracing.Orleans.Extensions.ServiceCollectionExtensions;
using ContextServiceCollectionExtensions = GiG.Core.Context.Orleans.Extensions.ServiceCollectionExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCorrelationAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CorrelationServiceCollectionExtensions.AddCorrelationAccessor(null));
        }

        [Fact]
        public void AddRequestContextAccessor_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ContextServiceCollectionExtensions.AddRequestContextAccessor(null));
        }
    }
}