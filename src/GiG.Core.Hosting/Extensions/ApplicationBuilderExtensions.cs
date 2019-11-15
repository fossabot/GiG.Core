using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Map the Info Management Endpoint to the Application.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseInfoManagement([NotNull] this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var options = app.ApplicationServices.GetService<IOptions<InfoManagementOptions>>()?.Value ?? new InfoManagementOptions();

            return app.Map(options.Url, appBuilder =>
            {                    
                appBuilder.Run(InfoManagementWriter.WriteJsonResponseWriter);
            });
        }
    }
}