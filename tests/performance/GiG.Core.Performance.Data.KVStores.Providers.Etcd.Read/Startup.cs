using dotnet_etcd;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            
            var etcdConfig = Configuration.GetSection("EtcdRead").Get<EtcdProviderOptions>();
            services.AddSingleton(new EtcdClient(etcdConfig.ConnectionString, 
                                                 etcdConfig.Port,
                                                 etcdConfig.Username, 
                                                 etcdConfig.Password, 
                                           etcdConfig.CaCertificate,
                                         etcdConfig.ClientCertificate, 
                                                 etcdConfig.ClientKey,
                                                 etcdConfig.IsPublicRootCa));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}