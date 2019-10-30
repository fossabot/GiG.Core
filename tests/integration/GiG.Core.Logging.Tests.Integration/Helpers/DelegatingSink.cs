using Serilog.Core;
using Serilog.Events;
using System;

namespace GiG.Core.Logging.Tests.Integration.Helpers
{
    public class DelegatingSink : ILogEventSink
    {
        private readonly Action<LogEvent> _write;

        public DelegatingSink(Action<LogEvent> write) => _write = write ?? throw new ArgumentNullException(nameof(write));

        public void Emit(LogEvent logEvent)
        {
            _write(logEvent);
        }
    }
}
