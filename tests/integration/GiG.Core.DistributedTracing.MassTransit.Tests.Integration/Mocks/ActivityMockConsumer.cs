using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks
{
    class ActivityMockConsumer : IConsumer<MockMessage>
    {
        public Task Consume(ConsumeContext<MockMessage> context)
        {
            State.Messages[context.Message.Id] = System.Diagnostics.Activity.Current.RootId;

            return Task.CompletedTask;
        }
    }
}
