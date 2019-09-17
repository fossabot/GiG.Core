using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Context.Orleans.Extensions
{
    /// <summary>
    /// Service Collection extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services for the Request Context Accessor functionality in Orleans.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <returns><see cref="IServiceCollection"/> so that more methods can be chained.</returns>
        public static IServiceCollection AddRequestContextAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IRequestContextAccessor, RequestContextAccessor>();

            return services;
        }
    }
}
