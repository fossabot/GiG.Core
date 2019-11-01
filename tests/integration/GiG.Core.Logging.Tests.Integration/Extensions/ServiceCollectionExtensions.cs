using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Logging.Tests.Integration.Mocks;
using GiG.Core.MultiTenant.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Logging.Tests.Integration.Extensions
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
        
        public static IServiceCollection AddMockCorrelationContextAccessor(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<ICorrelationContextAccessor>(new MockCorrelationContextAccessor());

            return services;
        }
    }
}
