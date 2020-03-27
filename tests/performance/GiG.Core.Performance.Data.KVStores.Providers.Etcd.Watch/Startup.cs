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
            var etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();
            var tlsEnabled = configuration.GetSection("EtcdWatch:TlsEnabled").Get<bool>();

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
            
            services.AddHostedService<HostedService>();
        }
    }
}