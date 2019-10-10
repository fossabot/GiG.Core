using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Http.Tests.Integration.Mocks
{
    internal class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        public MockCorrelationContextAccessor() => Value = Guid.NewGuid();

        public Guid Value { get; }
    }
}
