using GiG.Core.Hosting.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace GiG.Core.Hosting.AspNetCore.Extensions
{
    /// <summary>
    /// EndpointRouteBuilder Extensions.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds a Info Management endpoint to the <see cref="IEndpointRouteBuilder "/>.
        /// </summary>
        /// <param name="endpointRouteBuilder">The <see cref="IEndpointRouteBuilder"/>.</param>
        /// <returns>The <see cref="IEndpointConventionBuilder"/> that can be used to enrich the endpoint.</returns>
        public static IEndpointConventionBuilder MapInfoManagement([NotNull] this IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null) throw new ArgumentNullException(nameof(endpointRouteBuilder));

            var options = endpointRouteBuilder.ServiceProvider.GetService<IOptions<InfoManagementOptions>>()?.Value ?? new InfoManagementOptions();

            return endpointRouteBuilder.Map(options.Url, InfoManagementWriter.WriteJsonResponseWriter);
        }
    }
}
