using CorrelationId;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using System;

namespace GiG.Core.DistributedTracing.Web.Extensions
{
    /// <summary>
    /// Application Builder Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add Correlation ID Middleware.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/>.</returns>
        public static IApplicationBuilder UseCorrelationId([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var options = new CorrelationIdOptions
            {
                UseGuidForCorrelationId = true
            };

            return builder.UseCorrelationId(options);
        }
    }
}