using CorrelationId;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using CorrelationContextAccessor = GiG.Core.DistributedTracing.Web.CorrelationContextAccessor;
using ICorrelationContextAccessor = GiG.Core.DistributedTracing.Abstractions.CorrelationId.ICorrelationContextAccessor;

namespace GiG.Core.Extensions.DistributedTracing.Web
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support Correlation Id Functionality
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddCorrelationId(this IServiceCollection services)
        {
            CorrelationIdServiceExtensions.AddCorrelationId(services);
            services.TryAddSingleton<ICorrelationContextAccessor, CorrelationContextAccessor>();

            return services;
        }
    }
}