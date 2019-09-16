using JetBrains.Annotations;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Hosting.Extensions
{
    /// <summary>
    /// Host builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Registers Application Metadata.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder" />.</param>
        /// <returns>The <see cref="IHostBuilder" />.</returns>
        public static IHostBuilder UseApplicationMetadata([NotNull] this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.ConfigureServices((context, services) =>
            {
                var applicationName = context.Configuration["ApplicationName"];
                ApplicationMetadata.Name = applicationName;

                services.AddApplicationMetadataAccessor();
            });
        }
    }
}