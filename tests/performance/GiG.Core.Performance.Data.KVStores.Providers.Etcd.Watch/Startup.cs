using dotnet_etcd;
using Etcdserverpb;
using GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;
            
            var configurationSection =  configuration.GetSection("EtcdWatch");
            var etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();

            EtcdClient etcdClient = new EtcdClient(etcdProviderOptions.ConnectionString, etcdProviderOptions.Port,
                etcdProviderOptions.Username, etcdProviderOptions.Password, etcdProviderOptions.CaCertificate,
                etcdProviderOptions.ClientCertificate, etcdProviderOptions.ClientKey,
                etcdProviderOptions.IsPublicRootCa);

            etcdClient.Watch(etcdProviderOptions.Key, (WatchResponse response) =>
            {
                Console.WriteLine($"New Key {etcdProviderOptions.Key} Updated on {DateTime.UtcNow}");
            });
        }
    }
}