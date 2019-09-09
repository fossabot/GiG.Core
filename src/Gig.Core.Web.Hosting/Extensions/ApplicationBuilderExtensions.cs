using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Hosting.Extensions
{
    /// <summary>
    /// Application Builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        private const string DefaultPathBaseSectionName = "PATH_BASE";
     
        /// <summary>
        /// Configure the Path Base for the application using config key "PATH_BASE".
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder ConfigurePathBase(this IApplicationBuilder builder)
        {
            return builder.ConfigurePathBase(DefaultPathBaseSectionName);
        }

        /// <summary>
        /// Configure the Path Base for the application using configSection parameter.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <param name="configSection">Config key for path base.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder ConfigurePathBase(this IApplicationBuilder builder, string configSection)
        {
            var serviceProvider = builder.ApplicationServices;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var pathBase = configuration[configSection];
            
            if (!string.IsNullOrEmpty(pathBase))
            {
                builder.UsePathBase(pathBase);
            }

            return builder;
        }
        
    }
}