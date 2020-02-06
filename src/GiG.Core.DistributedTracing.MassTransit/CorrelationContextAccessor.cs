using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Messaging.MassTransit;
using System;

namespace GiG.Core.DistributedTracing.MassTransit
{
    /// <inheritdoc />
    internal class CorrelationContextAccessor : ICorrelationContextAccessor
    {
        private readonly IMassTransitContextAccessor _massTransitContextAccessor;

        public CorrelationContextAccessor(IMassTransitContextAccessor massTransitContextAccessor)
        {
            _massTransitContextAccessor = massTransitContextAccessor;
        }

        /// <inheritdoc />
        public Guid Value => _massTransitContextAccessor.MassTransitContext.CorrelationId;
    }
}
