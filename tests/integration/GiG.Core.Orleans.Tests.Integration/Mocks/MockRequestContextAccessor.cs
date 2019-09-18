using Bogus;
using GiG.Core.Context.Abstractions;
using System.Net;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockRequestContextAccessor : IRequestContextAccessor
    {
        private static readonly IPAddress RandomIPAddress = IPAddress.Parse(new Faker().Internet.Ip());

        public IPAddress IPAddress => RandomIPAddress;
    }
}