using GiG.Core.DistributedTracing.Activity.Extensions;
using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.MultiTenant.Activity.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.MultiTenant.Activity.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="ActivityTenantAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddActivityTenantAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services
                .AddActivityContextAccessor()
                .TryAddSingleton<ITenantAccessor, ActivityTenantAccessor>();

            return services;
        }
    }
}