using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.MultiTenant.Web.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.MultiTenant.Web.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to Tenant accessor functionality.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddTenantAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services
                .AddHttpContextAccessor()
                .TryAddSingleton<ITenantAccessor, TenantAccessor>();

            return services;
        }
    }
}