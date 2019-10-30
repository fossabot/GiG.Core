using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Orleans.Sample.Http.Services
{
    public class CorrelationContextAccessor: ICorrelationContextAccessor
    {
        public Guid Value { get; }

        public CorrelationContextAccessor()
        {
            Value = Guid.NewGuid();
        }
    }
}