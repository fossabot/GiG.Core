using GiG.Core.Providers.DateTime.Abstractions;
using System;

namespace GiG.Core.Providers.DateTime
{
    /// <summary>
    /// Providers a date time provider which returns the date and time in local timezone.
    /// </summary>
    public class LocalDateTimeProvider : IDateTimeProvider
    {
        /// <inheritdoc />
        public DateTimeOffset GetNow()
        {
            return DateTimeOffset.Now;
        }
    }
}