using GiG.Core.Data.Serializers.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Data.Serializers.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Json Data Serialization functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddSystemTextJsonDataSerializer<T>([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IDataSerializer<T>, JsonDataSerializer<T>>();

            return services;
        }

        /// <summary>
        /// Adds Xml Data Serialization functionality.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection AddXmlDataSerializer<T>([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IDataSerializer<T>, XmlDataSerializer<T>>();
            services.TryAddSingleton<IXmlDataSerializer<T>, XmlDataSerializer<T>>();
            services.TryAddSingleton<IXmlDataSerializer, XmlDataSerializer>();

            return services;
        }
    }
}