using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration
{
    public static class State
    {
        public static Dictionary<Guid, Guid> Messages;

        static State()
        {
            Messages = new Dictionary<Guid, Guid>();
        }
    }
}