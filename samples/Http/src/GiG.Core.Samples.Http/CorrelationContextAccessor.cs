using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Samples.Http
{
    internal class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        public CorrelationContextAccessor() => Value = Guid.NewGuid();

        public Guid Value { get; }
    }
}
