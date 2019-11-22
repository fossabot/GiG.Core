using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.FileProviders.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using System;
using System.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("GiG.Core.Data.Tests.Unit")]
namespace GiG.Core.Data.KVStores.Providers.FileProviders.Extensions
{
    /// <summary>
    /// KVStoreBuilder extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// FromJsonFile.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configuration">The <see cref="IConfiguration" /> which contains data to be consumed.</param>
        /// <param name="configurationSectionName">Configuration section name. </param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromJsonFile<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfiguration configuration, [NotNull] string configurationSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configurationSectionName)) throw new ArgumentException($"Missing {nameof(configurationSectionName)}.");

            return builder.FromJsonFile(configuration.GetSection(configurationSectionName));
        }
        
        /// <summary>
        /// FromJsonFile.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection" /> which contains data to be consumed.</param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromJsonFile<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ArgumentNullException(nameof(configurationSection));
            
            builder.Services.AddFileDataProvider();

            var fileProviderOptions = configurationSection.Get<FileProviderOptions>();
            if (fileProviderOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            builder.Services.AddSingleton<IDataProviderOptions<T, FileProviderOptions>>(
                new DataProviderOptions<T, FileProviderOptions>(fileProviderOptions));

            builder.Services.AddSingleton<IDataProvider<T>, JsonFileDataProvider<T>>();
            
            return builder;
        }
        
        /// <summary>
        /// AddFileDataProvider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>        
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        internal static IServiceCollection AddFileDataProvider([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            var physicalFileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
            services.TryAddSingleton<IFileProvider>(physicalFileProvider);

            return services;
        }
    }
}