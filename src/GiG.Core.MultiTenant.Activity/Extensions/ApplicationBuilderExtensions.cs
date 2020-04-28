using GiG.Core.MultiTenant.Activity.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using System;

namespace GiG.Core.MultiTenant.Activity.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds middleware which fills tenant information from http context to activity.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseTenantIdMiddleware([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseMiddleware<TenantIdMiddleware>();
        }
    }
}