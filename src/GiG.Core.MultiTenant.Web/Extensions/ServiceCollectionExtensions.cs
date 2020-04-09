using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.MultiTenant.Web.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.MultiTenant.Web.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="TenantAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddTenantAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services
                .AddHttpContextAccessor()
                .TryAddSingleton<ITenantAccessor, TenantAccessor>();

            return services;
        }
    }
}