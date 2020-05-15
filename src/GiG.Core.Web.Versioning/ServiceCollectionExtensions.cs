using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Web.Versioning
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds service API versioning and API explorer that is API version aware.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> on which to register the date time provider.</param>
        /// <param name="configureOptions">A delegate that is used to configure the <see cref="ApiVersioningOptions" />.</param>
        /// <returns></returns>
        public static IServiceCollection AddApiExplorerVersioning([NotNull] this IServiceCollection services, Action<ApiVersioningOptions> configureOptions = null)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;

                configureOptions?.Invoke(options);
            });
            
            services.AddVersionedApiExplorer(options =>
            {
                //The format of the version added to the route URL: "'v'major"
                options.GroupNameFormat = "'v'V";
                
                //Tells swagger to replace the version in the controller route  
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}