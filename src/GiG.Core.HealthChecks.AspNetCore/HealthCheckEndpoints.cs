using Microsoft.AspNetCore.Builder;

namespace GiG.Core.HealthChecks.AspNetCore.Internal
{
    /// <summary>
    /// Health Check Endpoints.
    /// </summary>
    public class HealthCheckEndpoints
    {
        /// <summary>
        /// Ready Health Check endpoint convention builder.
        /// </summary>
        public IEndpointConventionBuilder Ready { get; set; }

        /// <summary>
        /// Live Health Check endpoint convention builder.
        /// </summary>
        public IEndpointConventionBuilder Live { get; set; }

        /// <summary>
        /// Ready + Live Health Check endpoint convention builder.
        /// </summary>
        public IEndpointConventionBuilder Combined { get; set; }
    }
}
