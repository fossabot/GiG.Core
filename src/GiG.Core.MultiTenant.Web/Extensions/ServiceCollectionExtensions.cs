using GiG.Core.MultiTenant.Abstractions;
using GiG.Core.MultiTenant.Web.Internal;
using JetBrains.Annotations;
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
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>        
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddTenantAccessor([NotNull]this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services
                .AddHttpContextAccessor()
                .TryAddSingleton<ITenantAccessor, TenantAccessor>();

            return services;
        }
    }
}