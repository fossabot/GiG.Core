using GiG.Core.Context.Orleans.Extensions;
using GiG.Core.DistributedTracing.Orleans.Extensions;
using GiG.Core.DistributedTracing.Web.Extensions;
using GiG.Core.HealthChecks.Extensions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Orleans.Sample.Hubs;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.Hosting.Extensions;
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
            // Accessors
            services.AddSingleton<IPlayerInformationAccessor, PlayerInformationAccessor>();

            // Orleans Client
            services.AddClusterClient((builder, sp) =>
            {
                builder.UseSignalR();
                builder.AddCorrelationOutgoingFilter(sp);
                builder.AddRequestContextOutgoingFilter(sp);
                builder.ConfigureCluster(_configuration);
                builder.UseMembershipProvider(_configuration, x =>
                {
                    x.ConfigureConsulClustering(_configuration);
                    x.ConfigureKubernetesClustering(_configuration);
                });
                builder.AddAssemblies(typeof(IWalletGrain));

            });

            services.AddSignalR()
                    .AddOrleans();

            // Health Checks
            services.AddHealthChecks();

            // WebAPI
            services.ConfigureApiDocs(_configuration);
            services.ConfigureInfoManagement(_configuration);
            services.AddControllers();
            services.ConfigureForwardedHeaders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders();
            app.UsePathBaseFromConfiguration();
            app.UseCorrelationId();
            app.UseHealthChecks();
            app.UseApiDocs();
            app.UseInfoManagement();
            app.UseRouting();
            app.UseEndpoints(endpoints => 
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationsHub>("/notifications/open");
            });            
        }
    }
}