using System;

namespace GiG.Core.DistributedTracing.Abstractions.CorrelationId
{
    public interface ICorrelationContextAccessor
    {
        /// <summary>
        /// Contains the current context's correlation id
        /// </summary>
        Guid Value { get; }
    }
}
