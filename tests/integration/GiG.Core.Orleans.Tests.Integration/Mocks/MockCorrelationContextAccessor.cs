using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        private static readonly Guid RandomGuid = Guid.NewGuid();

        public Guid Value => RandomGuid;
    }
}