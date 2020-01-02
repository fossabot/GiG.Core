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
        /// <returns>The <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureExternalConfiguration([NotNull] this IHostBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
            return builder.ConfigureAppConfiguration(appConfig =>
            {
                if (Directory.Exists("./configs"))
                {
                    var configFiles = Directory.GetFiles("./configs", "*.json");

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