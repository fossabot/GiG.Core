using GiG.Core.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Formatting.Elasticsearch;

namespace GiG.Core.Logging.Sinks.File.Internal
{
    internal class FileLoggingSinkProvider : ILoggingSinkProvider
    {
        private readonly FileSinkOptions _options;

        public FileLoggingSinkProvider(IConfiguration configurationSection)
        {
            _options = configurationSection.Get<FileSinkOptions>() ?? new FileSinkOptions();
        }

        public void RegisterSink(LoggerSinkConfiguration sinkConfiguration)
        {
            sinkConfiguration.File(new ElasticsearchJsonFormatter(), _options.FilePath, rollingInterval: _options.RollingInterval, fileSizeLimitBytes: _options.FileSizeLimitBytes,
                retainedFileCountLimit: _options.RetainedFileCountLimit, rollOnFileSizeLimit: _options.RollOnFileSizeLimit);
        }
    }
}