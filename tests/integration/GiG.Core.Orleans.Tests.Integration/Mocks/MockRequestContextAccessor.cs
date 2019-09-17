using GiG.Core.Context.Abstractions;
using System;
using System.Net;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockRequestContextAccessor : IRequestContextAccessor
    {
        private static readonly Random randomizer = new Random();
        private static readonly IPAddress RandomIPAddress = 
            IPAddress.Parse(randomizer.Next(256) + "." + randomizer.Next(256) + "." + randomizer.Next(256) + "." + randomizer.Next(256));

        public IPAddress IPAddress => RandomIPAddress;
    }
}