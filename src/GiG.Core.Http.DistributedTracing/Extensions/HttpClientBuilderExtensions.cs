﻿using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Http.DistributedTracing.Extensions
{
    ///<summary>
    /// Http Client Builder Extensions.
    /// </summary>
    public static class HttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds the required services to support Correlation ID functionality.
        /// </summary>
        /// <param name="httpClientBuilder">The <see cref="IHttpClientBuilder"/>.</param>
        /// <returns>The <see cref="IHttpClientBuilder"/>.</returns>
        public static IHttpClientBuilder AddCorrelationIdDelegateHandler(
            [NotNull] this IHttpClientBuilder httpClientBuilder)
        {
            if (httpClientBuilder == null) throw new ArgumentNullException(nameof(httpClientBuilder));

            httpClientBuilder
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
                .Services.TryAddTransient<CorrelationIdDelegatingHandler>();

            return httpClientBuilder;
        }
    }
}