using CorrelationId;
using Microsoft.AspNetCore.Builder;

namespace GiG.Core.Extensions.DistributedTracing.Web
{
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Add correlation id middleware
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder)
        {
            var options = new CorrelationIdOptions {UseGuidForCorrelationId = true};

            return builder.UseCorrelationId(options);
        }
    }
}