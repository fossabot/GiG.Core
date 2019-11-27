using GiG.Core.Providers.DateTime.Abstractions;
using System;

namespace GiG.Core.Providers.DateTime
{
    /// <summary>
    /// Provides a mock date time provider which returns back the desired date time.
    /// </summary>
    public class MockDateTimeProvider : IDateTimeProvider //TODO: maybe rename GiG.Core.Web.Mock to GiG.Core.Mock and move there.
    {
        private readonly DateTimeOffset _desiredDateTimeOffset;

        /// <inheritdoc />
        public MockDateTimeProvider(DateTimeOffset desiredDateTimeOffset)
        {
            _desiredDateTimeOffset = desiredDateTimeOffset;
        }
        
        /// <inheritdoc />
        public DateTimeOffset GetNow() => _desiredDateTimeOffset;
    }
}