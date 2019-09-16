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
        /// Adds external configuration via JSON File and Environment Variables.
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <returns>Host builder.</returns>
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