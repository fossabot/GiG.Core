using GiG.Core.Extensions.DistributedTracing.Web;
using GiG.Core.Extensions.HealthCheck;
using GiG.Core.Web.Sample.Contracts;
using GiG.Core.Web.Sample.HealthChecks;
using GiG.Core.Web.Sample.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Sample
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationId();
            services.AddSingleton<ITransactionService, TransactionService>();
            services.Configure<TransactionSettings>(_configuration.GetSection("TransactionSettings"));
            services.AddCachedHealthChecks().AddCachedCheck<DummyCachedHealthCheck>(nameof(DummyCachedHealthCheck));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCorrelationId();
            app.UseRouting();
            app.UseHealthChecks();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
