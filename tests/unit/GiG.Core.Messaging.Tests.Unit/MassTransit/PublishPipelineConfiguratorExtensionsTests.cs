using GiG.Core.Messaging.MassTransit.Extensions;
using GiG.Core.Messaging.Tests.Unit.Mock;
using MassTransit;
using System;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace GiG.Core.Messaging.Tests.Unit.MassTransit
{
    [Trait("Category", "Unit")]
    public class PublishPipelineConfiguratorExtensionsTests
    {
        [Fact]
        public void UseFaultAddress_PublishPipelineConfiguratorIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => PublishPipelineConfiguratorExtensions.UseFaultAddress<MockMessage>(null, null));
            Assert.Equal("publishPipelineConfigurator", exception.ParamName);
        }

        [Fact]
        public void UseFaultAddress_FaultAddressIsNull_ThrowsArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => Bus.Factory.CreateUsingInMemory(cfg => { cfg.UseFaultAddress<MockMessage>(null); }) );
            Assert.Equal("faultAddress", exception.ParamName);
        }
    }
}
