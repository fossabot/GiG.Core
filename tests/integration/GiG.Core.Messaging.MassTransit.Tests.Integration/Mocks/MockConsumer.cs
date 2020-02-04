using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.Messaging.MassTransit.Tests.Integration.Mocks
{
    internal class MockConsumer : IConsumer<MockMessage>
    {
        public Task Consume(ConsumeContext<MockMessage> context)
        {
            State.FaultAddresses[context.Message.Id] = context.FaultAddress.ToString();
            return Task.CompletedTask;
        }
    }
}
