using System;

namespace GiG.Core.DistributedTracing.Abstractions.CorrelationId
{
    /// <summary>
    /// Correlation ID Accessor.
    /// </summary>
    public interface ICorrelationContextAccessor
    {
        /// <summary>
        /// Contains the current context's Correlation ID.
        /// </summary>
        Guid Value { get; }
    }
}
