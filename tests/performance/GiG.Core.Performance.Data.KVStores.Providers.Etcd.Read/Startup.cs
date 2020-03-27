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
            var tlsEnabled = Configuration.GetSection("EtcdRead:TlsEnabled").Get<bool>();

            var caCert = (tlsEnabled) ? File.ReadAllText("etcd-client-ca.crt") : "";
            var clientCert = (tlsEnabled) ? File.ReadAllText("etcd-client.crt") : "";
            var clientKey = (tlsEnabled) ? File.ReadAllText("etcd-client.key") : "";
            var isPublicRootCa = (tlsEnabled) && etcdProviderOptions.IsPublicRootCa;
         
            services.AddSingleton(new EtcdClient(etcdProviderOptions.ConnectionString, 
                etcdProviderOptions.Port,
                etcdProviderOptions.Username,
                etcdProviderOptions.Password,
                caCert,
                clientCert,
                clientKey,
                isPublicRootCa
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