using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Http.MultiTenant.Extensions
{
    ///<summary>
    /// Http Client Builder Extensions.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds the required services to support Tenant ID functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder"/>.</returns>
        public static IHttpClientBuilder AddTenantDelegatingHandler(
            [NotNull] this IHttpClientBuilder httpClientBuilder)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));

            httpClientBuilder
                .AddHttpMessageHandler<TenantDelegatingHandler>()
                .Services.TryAddTransient<TenantDelegatingHandler>();

            return httpClientBuilder;
        }
    }
}