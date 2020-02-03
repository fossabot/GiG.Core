using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GiG.Core.DistributedTracing.MassTransit.Tests.Integration
{
    public static class State
    {
        public static ConcurrentDictionary<Guid, string> Messages;

        private static Semaphore _initSemaphore = new Semaphore(1, 1);

        public static void Init()
        {
            _initSemaphore.WaitOne();
            if (Messages == null)
            {
                Messages = new ConcurrentDictionary<Guid, string>();
            }
            _initSemaphore.Release();
        }
    }
}