using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace GiG.Core.Configuration.Extensions
{
    /// <summary>
    /// Host Builder Extensions.
    /// </summary>
    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Adds external configuration files (.json files located in './configs' folder) and Environment Variables.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <param name="basePath">The base path of the config files</param>
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureExternalConfiguration([NotNull] this IHostBuilder builder, [NotNull] string basePath = "configs")
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.ConfigureAppConfiguration(appConfig =>
            {
                if (Directory.Exists(basePath))
                {
                    var configFiles = Directory.GetFiles(basePath, "*.json");

                    foreach (var configFile in configFiles)
                    {
                        appConfig.AddJsonFile(configFile, true, true);
                    }
                }
                appConfig.AddEnvironmentVariables();
            });
        }
    }
}