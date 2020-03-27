using dotnet_etcd;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using GiG.Core.Web.Docs.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureApiDocs(Configuration);
            
            services.AddControllers();

            var etcdProviderOptions = Configuration.GetSection("EtcdRead").Get<EtcdProviderOptions>();
            var isSslEnabled = Configuration.GetSection("EtcdRead:IsSslEnabled").Get<bool>();

            var caCert = (isSslEnabled) ? File.ReadAllText("etcd-client-ca.crt") : "";
            var clientCert = (isSslEnabled) ? File.ReadAllText("etcd-client.crt") : "";
            var clientKey = (isSslEnabled) ? File.ReadAllText("etcd-client.key") : "";

            services.AddSingleton(new EtcdClient(etcdProviderOptions.ConnectionString, 
                etcdProviderOptions.Port,
                etcdProviderOptions.Username,
                etcdProviderOptions.Password,
                caCert,
                clientCert,
                clientKey,
                etcdProviderOptions.IsPublicRootCa
            ));
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