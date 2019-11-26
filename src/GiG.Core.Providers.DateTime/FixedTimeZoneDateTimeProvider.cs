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
        
        /// <inheritdoc />
        public FixedTimeZoneDateTimeProvider(string timezoneId)
        {
            _timezone = TimeZoneInfo.FindSystemTimeZoneById(timezoneId);
        }
        
        /// <inheritdoc />
        public DateTimeOffset GetNow()
        {
            return TimeZoneInfo.ConvertTime(System.DateTime.UtcNow, _timezone);
        }
    }
}