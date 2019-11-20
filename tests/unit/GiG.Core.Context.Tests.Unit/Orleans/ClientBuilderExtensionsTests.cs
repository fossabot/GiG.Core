using GiG.Core.Context.Orleans.Extensions;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Context.Orleans.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Context.Tests.Unit.Orleans
{
    [Trait("Category", "Unit")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddRequestContextOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddRequestContextOutgoingFilter(null, null));
            Assert.Equal("builder", exception.ParamName);
        }

        [Fact]
        public void AddRequestContextOutgoingFilter_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new ClientBuilder().AddRequestContextOutgoingFilter(null));
            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}