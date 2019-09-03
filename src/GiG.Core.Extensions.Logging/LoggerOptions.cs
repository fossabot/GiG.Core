using System.Collections.Generic;
using GiG.Core.Extensions.Logging.Sinks;

namespace GiG.Core.Extensions.Logging
{
    internal class LoggerOptions
    {
        public IDictionary<string, BasicSinkOptions> Sinks { get; set; } = new Dictionary<string, BasicSinkOptions>();
    }
}