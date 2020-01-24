using GiG.Core.ApplicationMetrics.Abstractions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Prometheus;
using System;

namespace GiG.Core.ApplicationMetrics.Prometheus
{
    /// <summary>
    /// EndpointRouteBuilder Extensions.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Adds route for metrics.
        /// </summary>
        /// <param name="endpointRouteBuilder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IEndpointConventionBuilder MapApplicationMetrics(
            [NotNull] this IEndpointRouteBuilder endpointRouteBuilder)
        {
            if (endpointRouteBuilder == null) throw new ArgumentNullException(nameof(endpointRouteBuilder));

            var options =
                endpointRouteBuilder.ServiceProvider.GetService<IOptions<ApplicationMetricsOptions>>()?.Value ??
                new ApplicationMetricsOptions();

            if (!options.IsEnabled)
            {
                return null;
            }

            return endpointRouteBuilder.MapMetrics(options.Url);
        }
    }
}