using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Map the Info Management Endpoint to the Application.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseInfoManagement([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = builder.ApplicationServices.GetService<IOptions<InfoManagementOptions>>()?.Value ?? new InfoManagementOptions();

            return builder.Map(options.Url, appBuilder =>
            {                    
                appBuilder.Run(InfoManagementWriter.WriteJsonResponseWriter);
            });
        }
    }
}