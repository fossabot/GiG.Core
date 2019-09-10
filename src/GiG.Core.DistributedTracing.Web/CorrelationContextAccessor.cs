using System;
using GiG.Core.DistributedTracing.Abstractions;

namespace GiG.Core.DistributedTracing.Web
{
    /// <inheritdoc />
    internal class CorrelationContextAccessor : ICorrelationContextAccessor
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