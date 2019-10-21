using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Configuration.Extensions
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds external configuration via JSON File and Environment Variables.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureExternalConfiguration([NotNull] this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            return builder.ConfigureAppConfiguration(appConfig =>
            {
                appConfig.AddJsonFile("appsettings.override.json", true, true);
                appConfig.AddEnvironmentVariables();
            });
        }
    }
}