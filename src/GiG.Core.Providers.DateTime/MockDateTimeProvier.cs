using GiG.Core.Providers.DateTime.Abstractions;
using System;

namespace GiG.Core.Providers.DateTime
{
    /// <summary>
    /// Provides a mock date time provider which returns back the desired date time.
    /// </summary>
    public class MockDateTimeProvider : IDateTimeProvider //TODO: maybe rename GiG.Core.Web.Mock to GiG.Core.Mock and move there.
    {
        /// <summary>
        /// Initializes a new instance of the MockDateTimeProvider class.
        /// </summary>
        /// <param name="desiredDateTimeOffset">The date time offset.</param>
        public MockDateTimeProvider(DateTimeOffset desiredDateTimeOffset) => Now = desiredDateTimeOffset;

        /// <inheritdoc />
        public DateTimeOffset Now { get; }
    }
}