using Bogus;
using GiG.Core.Context.Abstractions;
using System.Net;

namespace GiG.Core.Logging.Tests.Integration.Mocks
{
    internal class RequestContextAccessor : IRequestContextAccessor
    {
        public IPAddress IPAddress { get; }

        public RequestContextAccessor() => IPAddress = new Faker().Internet.IpAddress();
    }
}
