using GiG.Core.Context.Web.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Helpers;
using GiG.Core.Logging.Tests.Integration.Tests;
using GiG.Core.MultiTenant.Web.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;

namespace GiG.Core.Logging.Tests.Integration.Fixtures
{
    public class LoggingFixture
    {
        public LogEvent LogEvent;
        public ILogger Logger;

        public LoggingFixture()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(x =>
                {
                    x.ConfigureForwardedHeaders();
                    x.AddCorrelationAccessor();
                    x.AddTenantAccessor();
                    x.AddRequestContextAccessor();
                })
                .ConfigureLogging(x => x
                    .WriteToSink(new DelegatingSink(e => LogEvent = e))
                    .EnrichWithApplicationMetadata()
                    .EnrichWithCorrelationId()
                    .EnrichWithTenantId()
                    .EnrichWithRequestContext()
                ).Build();

            host.StartAsync().GetAwaiter().GetResult();

            Logger = host.Services.GetRequiredService<ILogger<EnricherTests>>();

        }
    }
}
