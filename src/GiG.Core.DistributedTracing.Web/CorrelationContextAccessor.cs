using System;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.DistributedTracing.Abstractions.CorrelationId;

namespace GiG.Core.DistributedTracing.Web
{
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private readonly CorrelationId.ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationContextAccessor(CorrelationId.ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public Guid CorrelationId => Guid.TryParse(_correlationContextAccessor.CorrelationContext?.CorrelationId, out var correlationId) ? correlationId : Guid.NewGuid();
    }
}
