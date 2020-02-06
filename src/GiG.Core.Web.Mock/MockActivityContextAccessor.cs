using Bogus;
using GiG.Core.DistributedTracing.Abstractions;
using System.Collections.Generic;

namespace GiG.Core.Web.Mock
{
    /// <inheritdoc />
    public class MockActivityContextAccessor : IActivityContextAccessor
    {
        /// <summary>
        /// Initializes a new instance of the MockActivityContextAccessor class.
        /// </summary>
        public MockActivityContextAccessor()
        {
            var random = new Faker().Random;

            CorrelationId = random.Guid().ToString();
            TraceId = random.String2(16);
            SpanId = random.String2(8);
            ParentSpanId = random.String2(8);
            OperationName = random.String2(10);

            Baggage = new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>("TenantId", "1234")};
        }

        /// <inheritdoc />
        public string CorrelationId { get; }

        /// <inheritdoc />
        public IEnumerable<KeyValuePair<string, string>> Baggage { get; }

        /// <inheritdoc />
        public string TraceId { get; }

        /// <inheritdoc />
        public string SpanId { get; }

        /// <inheritdoc />
        public string ParentSpanId { get; }

        /// <inheritdoc />
        public string OperationName { get; }
    }
}