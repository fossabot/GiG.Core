using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.File.Sample.Models;
using GiG.Core.Data.KVStores.File.Sample.Services;
using GiG.Core.Data.KVStores.Providers.File.Extensions;
using GiG.Core.Data.KVStores.Providers.Hosting;
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

            services.AddKVStores<IEnumerable<Language>>(x =>
                x.FromFile(configuration, "Languages")
                    .WithEagerLoading());

            services.AddKVStores<IEnumerable<Currency>>(x =>
                x.FromFile(configuration, "Currencies"));

            services.AddHostedService<LanguageService>();
            services.AddHostedService<CurrencyService>();
        }
    }
}