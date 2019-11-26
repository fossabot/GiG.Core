using GiG.Core.TokenManager.Implementation;
using GiG.Core.TokenManager.Interfaces;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.TokenManager.Extensions
{
    public static class ServiceCollectionExtensions
    {
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