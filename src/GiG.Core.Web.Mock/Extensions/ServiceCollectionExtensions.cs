using GiG.Core.Context.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor;

namespace GiG.Core.Web.Mock.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
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
        /// Adds required services to support the <see cref="MockTenantAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddMockTenantAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ITenantAccessor>(new MockTenantAccessor());

            return services;
        }
        
        /// <summary>
        /// Adds required services to support the <see cref="MockCorrelationContextAccessor" /> functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>        
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection AddMockCorrelationAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ICorrelationContextAccessor>(new MockCorrelationContextAccessor());

            return services;
        }
    }
}
