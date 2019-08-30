using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.Extensions.Configuration
{
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds appsettings.override.json to HostBuilder
        /// </summary>
        /// <param name="builder">IHostBuilder</param>
        /// <returns>IHostBuilder</returns>
        public static IHostBuilder UseExternalConfiguration(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration(appConfig =>
            {
                appConfig.AddJsonFile("appsettings.override.json", optional: true, reloadOnChange: true);
                appConfig.AddEnvironmentVariables();
            });
        }
    }
}