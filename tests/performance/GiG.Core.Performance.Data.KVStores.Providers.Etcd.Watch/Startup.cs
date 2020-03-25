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
            services.Configure<EtcdProviderOptions>(configurationSection);

            services.AddSingleton(new EtcdClient(etcdProviderOptions.ConnectionString,
                etcdProviderOptions.Port,
                etcdProviderOptions.Username,
                etcdProviderOptions.Password,
                File.ReadAllText("etcd-client-ca.crt"),
                File.ReadAllText("etcd-client.crt"),
                File.ReadAllText("etcd-client.key"),
                etcdProviderOptions.IsPublicRootCa));

            services.AddHostedService<HostedService>();
        }
    }
}