using GiG.Core.Context.Orleans.Extensions;
using Orleans;
using System;
using Xunit;
using ClientBuilderExtensions = GiG.Core.Context.Orleans.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Context.Tests.Unit.Orleans
{
    [Trait("Category", "Integration")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddRequestContextOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ClientBuilderExtensions.AddRequestContextOutgoingFilter(null, null));
        }

        [Fact]
        public void AddRequestContextOutgoingFilter_ServiceCollectionIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ClientBuilder().AddRequestContextOutgoingFilter(null));
        }
    }
}