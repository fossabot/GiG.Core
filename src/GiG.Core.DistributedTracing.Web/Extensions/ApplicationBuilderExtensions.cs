using CorrelationId;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace GiG.Core.DistributedTracing.Web.Extensions
{
    /// <summary>
    /// Application Builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add Correlation ID Middleware.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseCorrelationId([NotNull] this IApplicationBuilder builder)
        {
            var options = new CorrelationIdOptions
            {
                UseGuidForCorrelationId = true
            };

            return builder.UseCorrelationId(options);
        }
    }
}