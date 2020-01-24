using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using GiG.Core.Data.KVStores.File.Sample.Models;
using GiG.Core.Data.KVStores.File.Sample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace GiG.Core.Data.KVStores.File.Sample
{
    internal static class Startup
    {
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;

            services.AddKVStores<IEnumerable<Language>>()
                .FromFile(configuration, "Languages")
                .WithJsonSerialization();

            services.AddKVStores<IEnumerable<Currency>>()
                .FromFile(configuration, "Currencies")
                .WithJsonSerialization();

            services.AddHostedService<LanguageService>();
            services.AddHostedService<CurrencyService>();
        }
    }
}