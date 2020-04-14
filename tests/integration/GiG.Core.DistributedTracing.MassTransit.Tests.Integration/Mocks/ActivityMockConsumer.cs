using GiG.Core.MultiTenant.Abstractions;
using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks
{
    internal class ActivityMockConsumer : IConsumer<MockMessage>
    {
        private readonly ITenantAccessor _tenantAccessor;

        public ActivityMockConsumer(ITenantAccessor tenantAccessor)
        {
            _tenantAccessor = tenantAccessor;
        }

        public Task Consume(ConsumeContext<MockMessage> context)
        {
            State.Messages[context.Message.Id] = new MessageContext
            {
                CorrelationId = System.Diagnostics.Activity.Current.RootId,
                TenantIds = _tenantAccessor.Values
            };

            return Task.CompletedTask;
        }
    }
}
