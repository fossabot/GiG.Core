using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Extensions.Configuration
{
    /// <summary>
    /// Host builder extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds external configuration via JSON File.
        /// </summary>
        /// <param name="builder">Host builder.</param>
        /// <returns>Host builder.</returns>
        public static IHostBuilder ConfigureExternalConfiguration([NotNull] this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration(appConfig =>
            {
                appConfig.AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true);
                appConfig.AddEnvironmentVariables();
            });
        }
    }
}