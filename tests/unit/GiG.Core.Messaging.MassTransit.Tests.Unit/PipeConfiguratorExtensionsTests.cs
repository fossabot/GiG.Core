using GiG.Core.Messaging.MassTransit.Extensions;
using MassTransit;
using MassTransit.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace GiG.Core.Messaging.MassTransit.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class PipeConfiguratorExtensionsTests
    {
        [Fact]
        public void UseActivityFilter_PipeConfiguratorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PipeConfiguratorExtensions.UseActivityFilter<InMemoryOutboxConsumeContext>(null));
            Assert.Equal("configurator", exception.ParamName);
        }

        [Fact]
        public void UseActivityFilterWithTracing_PipeConfiguratorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PipeConfiguratorExtensions.UseActivityFilterWithTracing<InMemoryOutboxConsumeContext>(null,null));
            Assert.Equal("configurator", exception.ParamName);
        }

        [Fact]
        public void UseActivityFilterWithTracing_ServiceProviderIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                var services = new ServiceCollection();
                services.AddMassTransit(x =>
                {
                    x.AddBus(provider => Bus.Factory.CreateUsingInMemory(cfg =>
                    {
                        cfg.ConfigurePublish(y => y.UseActivityFilterWithTracing(null));
                    }));
                });
                services.BuildServiceProvider().GetRequiredService<IBusControl>();
            });

            Assert.Equal("serviceProvider", exception.ParamName);
        }
    }
}
