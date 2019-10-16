using System.Collections.Generic;

namespace GiG.Core.Context.Orleans.Streams
{
    public abstract class EventMessageBase
    {
        public Dictionary<string, string> RequestContext { get; set; }
    }
}