using GiG.Core.Http.DistributedTracing.Extensions;
using GiG.Core.Http.Extensions;
using GiG.Core.Orleans.Sample.Http.Contracts;
using GiG.Core.Orleans.Sample.Http.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Refit;

namespace GiG.Core.Orleans.Sample.Http
{
    public static class Startup
    {
        public static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            services
                .AddHostedService<SampleService>();
              
            services.AddRefitClient<IWalletsClient>()
                .ConfigureHttpClient(c => c.FromConfiguration(ctx.Configuration, "Wallets"))
                .AddCorrelationContextDelegatingHandler()
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(3));

            services.AddRefitClient<IPaymentsClient>()
                .ConfigureHttpClient(c => c.FromConfiguration(ctx.Configuration, "Payments"))
                .AddCorrelationContextDelegatingHandler()
                .AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
        }
    }
}