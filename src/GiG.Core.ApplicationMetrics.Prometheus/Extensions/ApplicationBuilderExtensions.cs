using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Prometheus;
using System;

namespace GiG.Core.ApplicationMetrics.Prometheus.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds Http Metrics For Prometheus.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseHttpApplicationMetrics([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseHttpMetrics();
        }
    }
}