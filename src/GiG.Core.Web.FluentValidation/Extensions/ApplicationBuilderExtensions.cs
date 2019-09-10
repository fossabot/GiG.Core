using System;
using GiG.Core.Web.FluentValidation.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;

namespace GiG.Core.Web.FluentValidation.Extensions
{
    /// <summary>
    /// Application Builder extensions.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the Fluent Validation Exception Middleware.
        /// </summary>
        /// <param name="builder">Application builder.</param>
        /// <returns>Application builder.</returns>
        public static IApplicationBuilder UseFluentValidationMiddleware([NotNull] this IApplicationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.UseMiddleware<FluentValidationExceptionMiddleware>();
        }
    }
}