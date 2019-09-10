using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Web.Hosting.Extensions
{
    /// <summary>
    /// Service Collection Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configure Forward Headers.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the services to.</param>
        /// <returns>The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureForwardedHeaders(this IServiceCollection services)
        {
            return services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }
    }
}