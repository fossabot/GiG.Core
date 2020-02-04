using GiG.Core.Providers.DateTime.Abstractions;
using System;

namespace GiG.Core.Providers.DateTime
{
    /// <summary>
    /// Provides a date time provider which returns the current date time in the desired timezone.
    /// </summary>
    public class FixedTimeZoneDateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZoneInfo _timezone;
        
        /// <summary>
        /// Initializes a new instance of the FixedTimeZoneDateTimeProvider class.
        /// </summary>
        /// <param name="timezoneId">The time zone id.</param>
        public FixedTimeZoneDateTimeProvider(string timezoneId) => _timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);

        /// <inheritdoc />
        public DateTimeOffset Now => TimeZoneInfo.ConvertTime(System.DateTime.UtcNow, _timezone);
    }
}