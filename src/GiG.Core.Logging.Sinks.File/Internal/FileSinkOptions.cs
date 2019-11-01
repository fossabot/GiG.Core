using GiG.Core.Logging.Abstractions;
using Serilog;

namespace GiG.Core.Logging.Sinks.File.Internal
{
    internal class FileSinkOptions : BasicSinkOptions
    {
        public string FilePath { get; set; } = "logs\\log-.txt";

        public RollingInterval RollingInterval { get; set; } = RollingInterval.Day;

        public long? FileSizeLimitBytes { get; set; } = 1L * 1024 * 1024 * 1024;
        
        public int? RetainedFileCountLimit { get; set; } = 31;
        
        public bool RollOnFileSizeLimit { get; set; } = true;
    }
}