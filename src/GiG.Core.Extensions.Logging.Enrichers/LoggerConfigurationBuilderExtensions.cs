using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace GiG.Core.Extensions.Logging.Enrichers
{
    public static class LoggerConfigurationBuilderExtensions
    {
        public static LoggerConfigurationBuilder EnrichWithApplicationName(
            this LoggerConfigurationBuilder builder)
        {
            builder.Services.TryAddSingleton(x =>
                builder.LoggerConfiguration.Enrich.With(x.GetService<ApplicationNameEnricher>()));

            return builder;
        }
    }
}