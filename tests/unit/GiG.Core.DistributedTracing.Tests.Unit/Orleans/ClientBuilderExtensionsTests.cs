using GiG.Core.DistributedTracing.Orleans.Extensions;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.DistributedTracing.Orleans.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.DistributedTracing.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddCorrelationOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddCorrelationOutgoingFilter(null, null));
        }

        [Fact]
        public void AddCorrelationOutgoingFilter_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ClientBuilder().AddCorrelationOutgoingFilter(null));
        }
    }
}
