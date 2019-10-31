using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Logging.Tests.Integration.Mocks
{
    internal class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        public Guid Value { get; }

        public MockCorrelationContextAccessor()
        {
            Value = Guid.NewGuid();
        }
    }
}
