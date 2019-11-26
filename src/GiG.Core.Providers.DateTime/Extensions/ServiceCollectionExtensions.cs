using GiG.Core.Providers.DateTime.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
        public static void AddDateTimeProvider(this IServiceCollection services)
        {
            services.TryAddSingleton<IDateTimeProvider, UtcDateTimeProvider>();
        }
    }
}