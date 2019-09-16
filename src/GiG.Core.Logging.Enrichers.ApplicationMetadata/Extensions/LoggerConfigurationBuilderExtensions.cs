using GiG.Core.Logging.Abstractions;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;

namespace GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions
{
    /// <summary>
    /// Logger Configuration Builder extensions.
    /// </summary>
    public static class LoggerConfigurationBuilderExtensions
    {
        /// <summary>
        /// Enrich log events with a Application Metadata.
        /// </summary>
        /// <param name="builder">Logger enrichment configuration.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        public static LoggerConfigurationBuilder EnrichWithApplicationMetadata([NotNull] this LoggerConfigurationBuilder builder)
        {
            var configuration = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            var applicationVersion = Assembly
                .GetEntryAssembly()?
                .GetCustomAttributes<AssemblyInformationalVersionAttribute>().FirstOrDefault()?
                .InformationalVersion;
            
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationName", configuration["ApplicationName"]);
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationVersion", applicationVersion);

            return builder;
        }
    }
}