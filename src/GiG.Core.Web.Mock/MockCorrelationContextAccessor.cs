using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Web.Mock
{
    internal class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        public Guid Value { get; }

        public MockCorrelationContextAccessor()
        {
            Value = Guid.NewGuid();
        }
    }
}
