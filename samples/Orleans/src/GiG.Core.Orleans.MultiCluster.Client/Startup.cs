using GiG.Core.Hosting.Extensions;
using GiG.Core.Orleans.Client.Extensions;
using GiG.Core.Orleans.Clustering.Consul.Extensions;
using GiG.Core.Orleans.Clustering.Extensions;
using GiG.Core.Orleans.Clustering.Kubernetes.Extensions;
using GiG.Core.Orleans.Sample.Contracts;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.Hosting.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Orleans.MultiCluster.Client
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            //cluster clients can be added to the factory either via a created instance or an anonymous func.
            services.AddClusterClientFactory()
                .AddClusterClient("Payments", () =>
                {
                    return services.CreateClusterClient((builder) =>
                    {
                        builder.ConfigureCluster("Payments", _configuration);
                        builder.UseMembershipProvider(_configuration, x =>
                        {
                            x.ConfigureConsulClustering(_configuration);
                            x.ConfigureKubernetesClustering(_configuration);
                        });
                        builder.AddAssemblies(typeof(IEchoGrain));
                    });
                })
                .AddClusterClient("Games", () =>
                {
                    return services.CreateClusterClient((builder) =>
                    {
                        builder.ConfigureCluster("Games", _configuration);
                        builder.UseMembershipProvider(_configuration, x =>
                        {
                            x.ConfigureConsulClustering(_configuration);
                            x.ConfigureKubernetesClustering(_configuration);
                        });
                        builder.AddAssemblies(typeof(IEchoGrain));
                    });
                });
               
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
            app.UseApiDocs();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();             
            });
        }
    }
}
