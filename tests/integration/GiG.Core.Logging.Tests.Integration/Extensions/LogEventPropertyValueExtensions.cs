using Serilog.Events;
using System.Linq;

namespace GiG.Core.Logging.Tests.Integration.Extensions
{
    public static class Extensions
    {
        public static object LiteralValue(this LogEventPropertyValue @this) => ((ScalarValue)@this).Value;

        public static object[] SequenceValues(this LogEventPropertyValue @this)
        {
            if (@this is SequenceValue sequenceValue)
            {
                return sequenceValue.Elements.Select(x => x.LiteralValue()).ToArray();
            }

            return null;
        }
    }
}
