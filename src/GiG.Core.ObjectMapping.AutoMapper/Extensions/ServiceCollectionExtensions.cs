using AutoMapper;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;
using IObjectMapper = GiG.Core.ObjectMapping.Abstractions.IObjectMapper;

namespace GiG.Core.ObjectMapping.AutoMapper.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure IObjectMapper to use AutoMapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="assemblies">The Assemblies where the AutoMapper profiles are located.</param>
        public static void AddObjectMapper([NotNull] this IServiceCollection services, [NotNull] params Assembly[] assemblies)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (assemblies == null || assemblies.Length == 0) throw new ArgumentException(nameof(assemblies));

            services.AddAutoMapper(assemblies);
            services.TryAddScoped<IObjectMapper, AutoMapperObjectMapper>();
        }

        /// <summary>
        /// Configure IObjectMapper to use AutoMapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="profileAssemblyMarkerTypes">Marker types which tell AutoMapper where the profiles are located.</param>
        public static void AddObjectMapper([NotNull] this IServiceCollection services, [NotNull] params Type[] profileAssemblyMarkerTypes)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (profileAssemblyMarkerTypes == null || profileAssemblyMarkerTypes.Length == 0) throw new ArgumentException(nameof(profileAssemblyMarkerTypes));

            services.AddAutoMapper(profileAssemblyMarkerTypes);
            services.TryAddScoped<IObjectMapper, AutoMapperObjectMapper>();
        }
    }
}
