using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.MassTransit;
using System;

namespace GiG.Core.DistributedTracing.MassTransit
{
    /// <inheritdoc />
    public class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private readonly IMassTransitContextAccessor _massTransitContextAccessor;

        /// <inheritdoc />
        public CorrelationContextAccessor(IMassTransitContextAccessor massTransitContextAccessor)
        {
            _massTransitContextAccessor = massTransitContextAccessor;
        }

        /// <inheritdoc />
        public Guid Value => _massTransitContextAccessor.MassTransitContext.CorrelationId;
    }
}
