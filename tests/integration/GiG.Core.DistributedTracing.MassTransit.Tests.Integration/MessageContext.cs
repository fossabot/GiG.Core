using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration
{
    class MessageContext
    {
        public string CorrelationId { get; set; }
        public IEnumerable<string> TenantIds { get; set; } = new List<string>();
    }
}