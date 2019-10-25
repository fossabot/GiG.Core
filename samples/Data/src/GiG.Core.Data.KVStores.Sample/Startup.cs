using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using GiG.Core.Data.KVStores.Sample.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace GiG.Core.Data.KVStores.Sample
{
    internal static class Startup
    { 
        public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            var configuration = hostContext.Configuration;
            
            var physicalFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);

            services.AddOptions();
            
            services.Configure<FileProviderOptions>(configuration.GetSection("Data"))
                .AddSingleton<IFileProvider>(physicalFileProvider)
                .AddSingleton<IDataStore<IEnumerable<Person>>, MemoryDataStore<IEnumerable<Person>>>()
                .AddSingleton<IDataRetriever<IEnumerable<Person>>, DataRetriever<IEnumerable<Person>>>()
                .AddSingleton<IDataProvider<IEnumerable<Person>>, JsonFileDataProvider<IEnumerable<Person>>>();


            services.AddHostedService<ProviderHostedService<IEnumerable<Person>>>();
            services.AddHostedService<PersonService>();
        }
    }
}