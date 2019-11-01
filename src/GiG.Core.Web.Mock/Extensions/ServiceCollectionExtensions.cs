using CorrelationId;
using GiG.Core.Context.Abstractions;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.ICorrelationContextAccessor;

namespace GiG.Core.Web.Mock.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMockRequestContextAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IRequestContextAccessor>(new MockRequestContextAccessor());

            return services;
        }
        
        public static IServiceCollection AddMockTenantAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ITenantAccessor>(new MockTenantAccessor());

            return services;
        }
        
        public static IServiceCollection AddMockCorrelationAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCorrelationId();
            services.TryAddSingleton<ICorrelationContextAccessor>(new MockCorrelationContextAccessor());

            return services;
        }
    }
}
