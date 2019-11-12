using System;

namespace GiG.Core.MassTransit
{
    /// <summary>
    /// The MassTransit Context.
    /// </summary>
    public class MassTransitContext
    {
        /// <summary>
        /// The <see cref="MassTransitContext"/> constructor.
        /// </summary>
        /// <param name="correlationId">The CorrelationId of the message.</param>
        public MassTransitContext(Guid correlationId)
        {
            CorrelationId = correlationId == Guid.Empty ? Guid.NewGuid() : correlationId;
        }

        /// <summary>
        /// The Correlation Id of the current message.
        /// </summary>
        public Guid CorrelationId { get; }
    }
}
