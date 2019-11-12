using System;

namespace GiG.Core.MassTransit.Internal
{
    /// <summary>
    /// The MassTransitContext Factory.
    /// </summary>
    internal interface IMassTransitContextFactory : IDisposable
    {
        /// <summary>
        /// Creates a new <see cref="MassTransitContext"/> with the correlation Id set for the current message.
        /// </summary>
        /// <param name="correlationId">The correlation Id to set on the context.</param>
        /// <returns>A new instance of a <see cref="MassTransitContext"/>.</returns>
        MassTransitContext Create(Guid correlationId);
    }
}
