using GiG.Core.HealthChecks.Extensions;
using Microsoft.AspNetCore.Builder;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal  class MockStartupWithDefaultConfiguration
    {
        /// Defaults hard-coded on purpose
        /// If the default health checks urls are changed we need to infrom DevOps
        internal const string CombinedUrl = "/actuator/health";
        internal const string LiveUrl = "/actuator/health/live";
        internal const string ReadyUrl = "/actuator/health/ready";

        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks();
        }
    }
}
