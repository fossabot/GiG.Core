using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Client.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Web.Docs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddClusterClient((x, sp) =>
            {
                x.AddCorrelationOutgoingFilter(sp);
                x.ConfigureCluster(_configuration);
                x.ConfigureConsulClustering(_configuration);
                x.AddAssemblies(typeof(ITransactionGrain));
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