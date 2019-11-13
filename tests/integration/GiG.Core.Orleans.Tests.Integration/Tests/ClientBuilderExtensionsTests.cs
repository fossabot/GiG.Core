using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using Xunit;
using CorrelationClientBuilderExtensions = GiG.Core.DistributedTracing.Orleans.Extensions.ClientBuilderExtensions;
using RequestContextClientBuilderExtensions = GiG.Core.Context.Orleans.Extensions.ClientBuilderExtensions;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Orleans.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class ClientBuilderExtensionsTests
    {
        [Fact]
        public void AddCorrelationOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => CorrelationClientBuilderExtensions.AddCorrelationOutgoingFilter(null, null));
        }

        [Fact]
        public void AddCorrelationOutgoingFilter_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.AddCorrelationOutgoingFilter(null);
                    });
                }).Build());
        }

        [Fact]
        public void AddRequestContextOutgoingFilter_ClientBuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => RequestContextClientBuilderExtensions.AddRequestContextOutgoingFilter(null, null));
        }

        [Fact]
        public void AddRequestContextOutgoingFilter_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new HostBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDefaultClusterClient((x, sp) =>
                    {
                        x.AddRequestContextOutgoingFilter(null);
                    });
                }).Build());
        }
    }
}