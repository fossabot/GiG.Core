using Bogus;
using GiG.Core.Context.Abstractions;
using System.Net;

namespace GiG.Core.Web.Mock
{
    internal class MockRequestContextAccessor : IRequestContextAccessor
    {
        public IPAddress IPAddress { get; }

        public MockRequestContextAccessor()
        {
            IPAddress = new Faker().Internet.IpAddress();
        }
    }
}
