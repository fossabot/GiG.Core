using GiG.Core.Validation.FluentValidation.Web.Internal;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GiG.Core.Validation.FluentValidation.Web.Extensions
{
    /// <summary>
    /// The <see cref="IServiceCollection" /> Extensions.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a configuration instance which <see cref="ApiBehaviorOptions" /> will bind against.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" />.</param>
        /// <returns>The <see cref="IServiceCollection" />.</returns>
        public static IServiceCollection ConfigureApiBehaviorOptions([NotNull] this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            return services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var problemDetails = new ValidationProblemDetails(context.ModelState);
                    var result = new BadRequestObjectResult(problemDetails);

                    result.ContentTypes.Add(Constants.ProblemJsonMimeType);

                    return result;
                };
            });
        }
    }
}