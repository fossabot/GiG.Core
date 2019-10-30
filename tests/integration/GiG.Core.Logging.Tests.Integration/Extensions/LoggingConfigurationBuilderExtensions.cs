using GiG.Core.Logging.Abstractions;
using Serilog.Core;
using System;
using System.Diagnostics.CodeAnalysis;

namespace GiG.Core.Logging.Tests.Integration.Extensions
{
    public static class LoggingConfigurationBuilderExtensions
    {
        public static LoggingConfigurationBuilder WriteToSink([NotNull] this LoggingConfigurationBuilder builder, ILogEventSink logEventSink)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.LoggerConfiguration.WriteTo.Sink(logEventSink);

            return builder;
        }
    }
}
