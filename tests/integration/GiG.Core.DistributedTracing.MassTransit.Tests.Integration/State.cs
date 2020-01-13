using System;
using System.Collections.Generic;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration
{
    public static class State
    {
        public static Dictionary<Guid, string> Messages;

        public static void Init()
        {
            if (Messages == null)
            {
                Messages = new Dictionary<Guid, string>();
            }
        }
    }
}