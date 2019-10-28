using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;

namespace GiG.Core.Data.KVStores.Providers.FileProviders.Extensions
{
    public static class KVStoreBuilderExtensions
    {
        public static IKVStoreBuilder<T> AddJsonFile<T>(this IKVStoreBuilder<T> builder, IConfiguration configuration, string name)
        {
            builder.Services.AddFileDataProvider();

            builder.Services.AddSingleton<IDataProviderOptions<T, FileProviderOptions>>(
                new DataProviderOptions<T, FileProviderOptions>(configuration.GetSection(name)
                    .Get<FileProviderOptions>()));

            builder.Services.AddSingleton<IDataProvider<T>, JsonFileDataProvider<T>>();
            
            
            return builder;
        }
        
        private static IServiceCollection AddFileDataProvider(this IServiceCollection services)
        {
            var physicalFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            services.TryAddSingleton<IFileProvider>(physicalFileProvider);

            return services;
        }
    }
}