using GiG.Core.Data.KVStores.Etcd.Sample.Models;
using GiG.Core.Data.KVStores.Etcd.Sample.Services;
using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.Etcd.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace GiG.Core.Data.KVStores.Etcd.Sample
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.AddKVStores<IEnumerable<Language>>()
                .FromJsonEtcd(configuration, "Languages");

            services.AddKVStores<IEnumerable<Currency>>()
                .FromJsonEtcd(configuration, "Currencies");

            services.AddHostedService<LanguageService>();
            services.AddHostedService<CurrencyService>();
        }
    }
}