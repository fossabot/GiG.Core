using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
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
        /// <param name="assemblies">The Assemblies where the Automapper profiles are located.</param>
        public static void AddObjectMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(assemblies);
            services.TryAddScoped<IObjectMapper, AutoMapperObjectMapper>();
        }

        /// <summary>
        /// Configure IObjectMapper to use AutoMapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="assemblies">The Assemblies where the Automapper profiles are located.</param>
        /// <param name="serviceLifetime">Specifies the lifetime of the services in the <see cref="IServiceCollection"/>.</param>
        public static void AddObjectMapper(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        {
            services.AddAutoMapper(assemblies, serviceLifetime);
            services.TryAddScoped<IObjectMapper, AutoMapperObjectMapper>();
        }

        /// <summary>
        /// Configure IObjectMapper to use AutoMapper.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="profileAssemblyMarkerTypes">Marker types which tell AutoMapper where the profiles are located.</param>
        public static void AddObjectMapper(this IServiceCollection services, params Type[] profileAssemblyMarkerTypes)
        {
            services.AddAutoMapper(profileAssemblyMarkerTypes);
            services.TryAddScoped<IObjectMapper, AutoMapperObjectMapper>();
        }
    }
}
