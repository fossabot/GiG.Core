using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.DistributedTracing.Web.Internal
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