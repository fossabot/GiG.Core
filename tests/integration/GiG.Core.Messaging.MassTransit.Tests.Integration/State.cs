using System;
using System.Collections.Generic;

namespace GiG.Core.Messaging.MassTransit.Tests.Integration
{
    public static class State
    {
        public static Dictionary<Guid, string> FaultAddresses;

        static State()
        {
            FaultAddresses = new Dictionary<Guid, string>();
        }
    }
}