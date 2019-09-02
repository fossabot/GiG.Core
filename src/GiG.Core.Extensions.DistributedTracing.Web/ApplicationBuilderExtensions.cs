using CorrelationId;
using Microsoft.AspNetCore.Builder;

namespace GiG.Core.Extensions.DistributedTracing.Web
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseGiGCorrelationId(this IApplicationBuilder builder)
        {
            var options = new CorrelationIdOptions {UseGuidForCorrelationId = true};

            return builder.UseCorrelationId(options);
        }
    }
}