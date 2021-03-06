﻿using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Etcd.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Configuration;

namespace GiG.Core.Data.KVStores.Providers.Etcd.Extensions
{
    /// <summary>
    /// The <see cref="IKVStoreBuilder{T}" /> extensions.
    /// </summary>
    public static class KVStoreBuilderExtensions
    {
        /// <summary>
        /// Registers an etcd KV Store.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="EtcdProviderOptions"/>.</param>
        /// <param name="configurationSectionName">Configuration section name. </param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromEtcd<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfiguration configuration, [NotNull] string configurationSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (string.IsNullOrWhiteSpace(configurationSectionName)) throw new ArgumentException($"'{nameof(configurationSectionName)}' must not be null, empty or whitespace.", nameof(configurationSectionName));

            return builder.FromEtcd(configuration.GetSection(configurationSectionName));
        }

        /// <summary>
        /// Registers an etcd KV Store.
        /// </summary>
        /// <param name="builder">The <see cref="IKVStoreBuilder{T}" /> to add the services to.</param>        
        /// <param name="configurationSection">The <see cref="IConfigurationSection"/> which binds to <see cref="EtcdProviderOptions"/>.</param>
        /// <typeparam name="T">Generic to define type of KVStoreBuilder. </typeparam>
        /// <returns>The <see cref="IKVStoreBuilder{T}" /> so that additional calls can be chained.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IKVStoreBuilder<T> FromEtcd<T>([NotNull] this IKVStoreBuilder<T> builder, [NotNull] IConfigurationSection configurationSection)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configurationSection?.Exists() != true) throw new ConfigurationErrorsException($"Configuration section '{configurationSection?.Path}' is incorrect.");

            var etcdProviderOptions = configurationSection.Get<EtcdProviderOptions>();
            if (etcdProviderOptions == null)
            {
                throw new ConfigurationErrorsException($"Configuration section '{configurationSection.Path}' is not valid.");
            }

            if (string.IsNullOrEmpty(etcdProviderOptions.Key))
            {
                throw new ConfigurationErrorsException($"Root key '{etcdProviderOptions.Key}' in '{configurationSection.Path}' is missing.");
            }

            builder.Services.TryAddSingleton<IDataProviderOptions<T, EtcdProviderOptions>>(new DataProviderOptions<T, EtcdProviderOptions>(etcdProviderOptions));
            builder.RegisterDataProvider<EtcdDataProvider<T>>();

            return builder;
        }
    }
}