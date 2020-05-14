using GiG.Core.Logging.AspNetCore.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Logging.AspNetCore.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Logging for Http Request and Http Response.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseHttpRequestResponseLogging(
            [NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var httpRequestResponseLoggingOptions = builder.ApplicationServices.GetService<IOptions<HttpRequestResponseLoggingOptions>>()?.Value ?? new HttpRequestResponseLoggingOptions();

            if (httpRequestResponseLoggingOptions.IsEnabled)
            {
                return builder.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
            }

            return builder;
        }
    }
}