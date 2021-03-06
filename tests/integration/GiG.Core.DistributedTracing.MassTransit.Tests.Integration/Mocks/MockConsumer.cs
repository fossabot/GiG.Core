﻿using GiG.Core.DistributedTracing.Abstractions;
using MassTransit;
using System.Threading.Tasks;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration.Mocks
{
    internal class MockConsumer : IConsumer<MockMessage>
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public MockConsumer(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public Task Consume(ConsumeContext<MockMessage> context)
        {
            var correlationId = _correlationContextAccessor.Value;
            State.Messages[context.Message.Id] = new MessageContext { CorrelationId = correlationId.ToString() };

            return Task.CompletedTask;
        }
    }
}
