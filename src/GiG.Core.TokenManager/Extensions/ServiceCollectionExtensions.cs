using GiG.Core.TokenManager.Implementation;
using GiG.Core.TokenManager.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.TokenManager.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the required services for the Token Manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddTokenManager([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.AddDateTimeProvider();
            services.AddHttpClient();

            services.TryAddSingleton<ITokenClientFactory, TokenClientFactory>();
            services.TryAddSingleton<ITokenManagerFactory, TokenManagerFactory>();

            return services;
        }
    }
}