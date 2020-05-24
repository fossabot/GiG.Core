using dotnet_etcd;
using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using GiG.Core.Data.Serializers.Abstractions;
using GiG.Core.Web.Docs.Extensions;
using GiG.Core.Web.Versioning.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
            services.ConfigureApiDocs(Configuration)
                .AddApiExplorerVersioning();

            services.AddControllers();

            var etcdConfigurationSection = Configuration.GetSection("EtcdRead");
            var etcdProviderOptions = etcdConfigurationSection.Get<EtcdProviderOptions>();

            services.Configure<EtcdProviderOptions>(etcdConfigurationSection);
            services.TryAddSingleton(new EtcdClient(etcdProviderOptions.ConnectionString, etcdProviderOptions.Port,
                etcdProviderOptions.Username, etcdProviderOptions.Password, etcdProviderOptions.CaCertificate,
                etcdProviderOptions.ClientCertificate, etcdProviderOptions.ClientKey,
                etcdProviderOptions.IsPublicRootCa));

            services.AddSingleton<IDataProviderOptions<string, EtcdProviderOptions>>(new DataProviderOptions<string, EtcdProviderOptions>(etcdProviderOptions));
            services.AddSingleton<IDataSerializer<string>, StringSerializer>();
            services.AddSingleton<IDataProvider<string>, EtcdDataProvider<string>>();
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