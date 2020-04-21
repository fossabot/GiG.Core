using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.File.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;

namespace GiG.Core.Data.KVStores.Providers.File.Extensions
{
    /// <summary>
    /// The <see cref="IKVStoreBuilder{T}" /> extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// Registers a KV Store from File.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="FileProviderOptions"/>.</param>
        /// <param name="configurationSectionName">Configuration section name. </param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromFile<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfiguration configuration, [NotNull] string configurationSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configurationSectionName)) throw new ArgumentException($"'{nameof(configurationSectionName)}' must not be null, empty or whitespace.", nameof(configurationSectionName));

            return builder.FromFile(configuration.GetSection(configurationSectionName));
        }

        /// <summary>
        /// Registers a KV Store from File.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="FileProviderOptions"/>.</param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromFile<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");
            
            var fileProviderOptions = configurationSection.Get<FileProviderOptions>();
            if (fileProviderOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            builder.Services.TryAddSingleton<IDataProviderOptions<T, FileProviderOptions>>(new DataProviderOptions<T, FileProviderOptions>(fileProviderOptions));
            builder.RegisterDataProvider<FileDataProvider<T>>();

            return builder;
        }
    }
}