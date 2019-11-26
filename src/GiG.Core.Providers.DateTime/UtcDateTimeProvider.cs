using GiG.Core.Providers.DateTime.Abstractions;
using System;

namespace GiG.Core.Providers.DateTime
{
    /// <summary>
    /// Provides a date time provider which returns the current date and time in UTC.
    /// </summary>
    public class UtcDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTimeOffset GetNow()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}