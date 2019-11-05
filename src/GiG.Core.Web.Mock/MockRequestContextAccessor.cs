using Bogus;
using GiG.Core.Context.Abstractions;
using System.Net;

namespace GiG.Core.Web.Mock
{
    /// <inheritdoc />
    public class MockRequestContextAccessor : IRequestContextAccessor
    {
        /// <inheritdoc />
        public IPAddress IPAddress { get; }

        /// <inheritdoc />
        public MockRequestContextAccessor()
        {
            IPAddress = new Faker().Internet.IpAddress();
        }
    }
}
