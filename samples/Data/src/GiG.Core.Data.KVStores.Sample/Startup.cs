using GiG.Core.Data.KVStores.Extensions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Extensions;
using GiG.Core.Data.KVStores.Sample.Models;
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

            services.AddDataProvider<IEnumerable<Person>>()
                .AddJsonFile(configuration, "People");

            services.AddDataProvider<IEnumerable<Animal>>()
                .AddJsonFile(configuration, "Animals");
            
            services.AddHostedService<PersonService>();
            services.AddHostedService<AnimalService>();
        }
    }
}