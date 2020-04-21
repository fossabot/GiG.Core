using GiG.Core.Validation.FluentValidation.Web.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using System;

namespace GiG.Core.Validation.FluentValidation.Web.Extensions
{
    /// <summary>
    /// The <see cref="IApplicationBuilder" /> Extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Fluent Validation Exception Middleware.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder" />.</param>
        /// <returns>The <see cref="IApplicationBuilder" />.</returns>
        public static IApplicationBuilder UseFluentValidationMiddleware([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseMiddleware<FluentValidationExceptionMiddleware>();
        }
    }
}