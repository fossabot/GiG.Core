using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using GiG.Core.Data.KVStores.Sample.Models;
using GiG.Core.Data.KVStores.Sample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace GiG.Core.Data.KVStores.Sample
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.AddKVStores<IEnumerable<Language>>()
                .AddJsonFile(configuration, "Languages");

            services.AddKVStores<IEnumerable<Currency>>()
                .AddJsonFile(configuration, "Currencies");
            
            services.AddHostedService<LanguageService>();
            services.AddHostedService<CurrencyService>();
        }
    }
}