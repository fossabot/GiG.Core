using Microsoft.Extensions.Logging;

namespace GiG.Common.Extensions.Logging
{
    public class LoggerConfig
    {
        public LogLevel MinimumLogLevel { get; set; }

        public bool LogToConsole { get; set; }
    }
}
