using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Web.Hosting.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string DefaultPathBaseSectionName = "PATH_BASE";
     
        /// <summary>
        /// Configures the Path Base for the application using the config key "PATH_BASE".
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UsePathBaseFromConfiguration([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.UsePathBaseFromConfiguration(DefaultPathBaseSectionName);
        }

        /// <summary>
        /// Configures the Path Base for the application using configSectionName parameter.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <param name="configSectionName">The Config section name.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UsePathBaseFromConfiguration([NotNull] this IApplicationBuilder builder, [NotNull] string configSectionName)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (configSectionName == null) throw new ArgumentNullException(nameof(configSectionName));
            
            var serviceProvider = builder.ApplicationServices;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var pathBase = configuration[configSectionName];
            
            if (!string.IsNullOrEmpty(pathBase))
            {
                builder.UsePathBase(pathBase);
            }

            return builder;
        }
    }
}