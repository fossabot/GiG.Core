using Bogus;
using GiG.Core.Context.Abstractions;
using System.Net;

namespace GiG.Core.Logging.Tests.Integration.Mocks
{
    internal class RequestContextAccessor : IRequestContextAccessor
    {
        private static IPAddress _ipAddress;
        public IPAddress IPAddress
        {
            get { return _ipAddress ??= new Faker().Internet.IpAddress(); }
        }
    }
}
