using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;

namespace GiG.Core.Data.KVStores.Providers.FileProviders.Extensions
{
    /// <summary>
    /// KVStoreBuilder extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// AddJsonFile.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains data to be consumed.</param>
        /// <param name="configurationSectionName">Configuration section name. </param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> AddJsonFile<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfiguration configuration, [NotNull] string configurationSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configurationSectionName)) throw new ArgumentNullException(nameof(configurationSectionName));
            
            builder.Services.AddFileDataProvider();

            builder.Services.AddSingleton<IDataProviderOptions<T, FileProviderOptions>>(
                new DataProviderOptions<T, FileProviderOptions>(configuration.GetSection(configurationSectionName)
                    .Get<FileProviderOptions>()));

            builder.Services.AddSingleton<IDataProvider<T>, JsonFileDataProvider<T>>();
            
            
            return builder;
        }
        
        /// <summary>
        /// AddFileDataProvider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>        
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IServiceCollection AddFileDataProvider([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var physicalFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            services.TryAddSingleton<IFileProvider>(physicalFileProvider);

            return services;
        }
    }
}