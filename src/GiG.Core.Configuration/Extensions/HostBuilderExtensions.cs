using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace GiG.Core.Configuration.Extensions
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