using System;

namespace GiG.Core.Providers.DateTime.Abstractions
{
    /// <summary>
    /// Used to get the current date time based on the registered implementation.
    /// </summary>
    public interface IDateTimeProvider
    {
        /// <summary>
        /// Returns the current date and time.
        /// </summary>
        /// <returns><see cref="DateTimeOffset"/></returns>
        DateTimeOffset GetNow();
    }
}