using dotnet_etcd;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            var configurationSection = configuration.GetSection("EtcdWatch");
            var isSslEnabled = configuration.GetSection("EtcdWatch:IsSslEnabled").Get<bool>();
            var etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();
            services.Configure<EtcdProviderOptions>(configurationSection);
           

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
            
            services.AddHostedService<HostedService>();
        }
    }
}