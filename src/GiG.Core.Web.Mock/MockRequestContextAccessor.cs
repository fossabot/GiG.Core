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

        /// <summary>
        /// Initializes a new instance of the MockRequestContextAccessor class.
        /// </summary>
        public MockRequestContextAccessor()
        {
            IPAddress = new Faker().Internet.IpAddress();
        }
    }
}
