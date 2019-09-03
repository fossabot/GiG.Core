using GiG.Core.DistributedTracing.Abstractions.CorrelationId;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GiG.Core.Extensions.Logging.Enrichers
{
    public static class LoggerConfigurationBuilderExtensions
    {
        public static LoggerConfigurationBuilder EnrichWithApplicationMetadata(this LoggerConfigurationBuilder builder)
        {
            var config = builder.Services.BuildServiceProvider().GetService<IConfiguration>();
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationName", config["ApplicationName"]);
            builder.LoggerConfiguration.Enrich.WithProperty("ApplicationVersion", "1.0.0");

            return builder;
        }

        public static LoggerConfigurationBuilder EnrichWithCorrelationId(
            this LoggerConfigurationBuilder builder)
        {
            var correlationContextAccessor = builder.Services.BuildServiceProvider().GetService<ICorrelationContextAccessor>();
            builder.LoggerConfiguration.Enrich.With(new CorrelationIdEnricher(correlationContextAccessor));

            return builder;
        }
    }
}