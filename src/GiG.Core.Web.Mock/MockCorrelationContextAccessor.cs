using GiG.Core.DistributedTracing.Abstractions;
using System;

namespace GiG.Core.Web.Mock
{
    /// <inheritdoc />
    public class MockCorrelationContextAccessor : ICorrelationContextAccessor
    {
        /// <inheritdoc />
        public Guid Value { get; }

        /// <summary>
        /// Initializes a new instance of the MockCorrelationContextAccessor class.
        /// </summary>
        public MockCorrelationContextAccessor()
        {
            Value = Guid.NewGuid();
        }
    }
}
