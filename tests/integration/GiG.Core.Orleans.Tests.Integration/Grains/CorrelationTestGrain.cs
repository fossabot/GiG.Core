using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Orleans.Tests.Integration.Contracts;
using Orleans;
using System;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Tests.Integration.Grains
{
    public class CorrelationTestGrain : Grain, ICorrelationTestGrain
    {
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationTestGrain(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public Task<Guid> GetCorrelationIdAsync()
        {
            return Task.FromResult(_correlationContextAccessor.Value);
        }
    }
}