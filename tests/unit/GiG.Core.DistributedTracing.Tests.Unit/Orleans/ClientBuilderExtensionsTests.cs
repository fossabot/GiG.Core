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
        public void AddActivityOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddActivityOutgoingFilter(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddActivityOutgoingFilter_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().AddActivityOutgoingFilter(null));
            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}
