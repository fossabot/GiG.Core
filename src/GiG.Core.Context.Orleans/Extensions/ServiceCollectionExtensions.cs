﻿using GiG.Core.Context.Abstractions;
using GiG.Core.Context.Orleans.Internal;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace GiG.Core.Context.Orleans.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds required services to support the <see cref="RequestContextAccessor"/> functionality in Orleans.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddRequestContextAccessor([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.TryAddSingleton<IRequestContextAccessor, RequestContextAccessor>();

            return services;
        }
    }
}
