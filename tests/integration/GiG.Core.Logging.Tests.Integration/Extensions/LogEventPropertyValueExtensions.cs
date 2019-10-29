using Serilog.Events;

namespace GiG.Core.Logging.Tests.Integration.Extensions
{
    public static class Extensions
    {
        public static object LiteralValue(this LogEventPropertyValue @this) => ((ScalarValue)@this).Value;
    }
}
