using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Web.Mock.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="MockActivityTenantAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddMockActivityTenantAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ITenantAccessor>(new MockActivityTenantAccessor());

            return services;
        }

        /// <summary>
        /// Adds required services to support the <see cref="MockRequestContextAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddMockRequestContextAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IRequestContextAccessor>(new MockRequestContextAccessor());

            return services;
        }
        
        /// <summary>
        /// Adds required service to support <see cref="MockActivityContextAccessor"/> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> to chain additional calls.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IServiceCollection AddMockActivityContextAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IActivityContextAccessor>(new MockActivityContextAccessor());

            return services;
        }
    }
}
