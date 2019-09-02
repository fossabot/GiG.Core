using System;

namespace GiG.Core.DistributedTracing.Abstractions.CorrelationId
{
    public interface ICorrelationContextAccessor
    {
        Guid CorrelationId { get; }
    }
}
