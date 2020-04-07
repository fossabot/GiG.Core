using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration
{
    static class State
    {
        public static ConcurrentDictionary<Guid, MessageContext> Messages;

        private static readonly Semaphore InitSemaphore = new Semaphore(1, 1);

        public static void Init()
        {
            InitSemaphore.WaitOne();
            if (Messages == null)
            {
                Messages = new ConcurrentDictionary<Guid, MessageContext>();
            }
            InitSemaphore.Release();
        }
    }
}