using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Http.Tests.Integration
{
    internal class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        public CorrelationContextAccessor() => Value = Guid.NewGuid();

        public Guid Value { get; }
    }
}
