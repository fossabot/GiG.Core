using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Web.Mock
{
    /// <inheritdoc />
    public class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        /// <inheritdoc />
        public Guid Value { get; }

        /// <inheritdoc />
        public MockCorrelationContextAccessor()
        {
            Value = Guid.NewGuid();
        }
    }
}
