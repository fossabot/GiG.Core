using GiG.Core.Providers.DateTime.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Providers.DateTime.Extensions
{
    /// <summary>
    /// Date Time Provider Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registerer a Utc Date Time Provider.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> on which to register the date time provider.</param>
        public static void AddUtcDateTimeProvider([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            
            services.TryAddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        }
    }
}