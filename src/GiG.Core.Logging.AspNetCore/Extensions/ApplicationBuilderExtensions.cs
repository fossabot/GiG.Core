using GiG.Core.Logging.AspNetCore.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
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
        /// <param name="configuration">The <see cref="IConfiguration"/> which binds to <see cref="HttpRequestResponseLoggingOptions"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseHttpRequestResponseLogging(
            [NotNull] this IApplicationBuilder builder,
            [NotNull] IConfiguration configuration)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var httpRequestResponseLoggingOptions = configuration.Get<HttpRequestResponseLoggingOptions>() ?? new HttpRequestResponseLoggingOptions();

            if (httpRequestResponseLoggingOptions.IsEnabled)
            {
                return builder.UseMiddleware<HttpRequestResponseLoggingMiddleware>();
            }

            return builder;
        }
    }
}