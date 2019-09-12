using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Client.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Web.Docs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace GiG.Core.Orleans.Sample.Client
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
            services.AddClusterClient((cb, sp) =>
            {
                cb.AddCorrelationOutgoingFilter(sp);
                cb.ConfigureCluster(_configuration);
                cb.ConfigureConsulClustering(_configuration);
                cb.AddAssemblies(typeof(ITransactionGrain));
            });

            services.AddControllers();

            services.ConfigureApiDocs(_configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseApiDocs();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}