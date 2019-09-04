using System;
using GiG.Core.DistributedTracing.Abstractions.CorrelationId;

namespace GiG.Core.DistributedTracing.Web
{
    /// <inheritdoc />
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private readonly CorrelationId.ICorrelationContextAccessor _correlationContextAccessor;

        /// <inheritdoc />
        public CorrelationContextAccessor(CorrelationId.ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        /// <inheritdoc />
        public Guid Value =>
            Guid.TryParse(_correlationContextAccessor.CorrelationContext?.CorrelationId, out var correlationId)
                ? correlationId
                : Guid.NewGuid();
    }
}