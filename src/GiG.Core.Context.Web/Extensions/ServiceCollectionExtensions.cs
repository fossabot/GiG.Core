using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Web.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Context.Web.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services for the Request Context Accessor functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>        
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddRequestContextAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services
                .AddHttpContextAccessor()
                .TryAddSingleton<IRequestContextAccessor, RequestContextAccessor>();

            return services;
        }
    }
}
