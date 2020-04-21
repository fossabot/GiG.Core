using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Http.DistributedTracing.Extensions
{
    /// <summary>
    /// The <see cref="IHttpClientBuilder" /> Extensions.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds the required services to support Correlation ID functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder"/>.</returns>
        public static IHttpClientBuilder AddCorrelationContextDelegatingHandler(
            [NotNull] this IHttpClientBuilder httpClientBuilder)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));

            httpClientBuilder
                .AddHttpMessageHandler<CorrelationContextDelegatingHandler>()
                .Services.TryAddTransient<CorrelationContextDelegatingHandler>();

            return httpClientBuilder;
        }
    }
}