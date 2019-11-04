using GiG.Core.HealthChecks.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.HealthChecks.Tests.Integration.Mocks
{
    internal  class MockStartupWithDefaultConfiguration
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseHealthChecks();
        }
    }
}
