using CorrelationId;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CorrelationContextAccessor = GiG.Core.DistributedTracing.Web.CorrelationContextAccessor;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.CorrelationId.ICorrelationContextAccessor;

namespace GiG.Core.Extensions.DistributedTracing.Web
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGiGCorrelationId(this IServiceCollection services)
        {
            services.AddCorrelationId();
            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}