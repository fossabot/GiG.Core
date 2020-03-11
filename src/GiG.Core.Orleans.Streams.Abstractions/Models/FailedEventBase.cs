using System;

namespace GiG.Core.Orleans.Streams.Abstractions.Models
{
    /// <summary>
    /// The Failed Event Base class.
    /// </summary>
    public abstract class FailedEventBase
    {
        /// <summary>
        /// The Failed Event Id.
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// The Error Code.
        /// </summary>
        public string ErrorCode { get; set; }
        /// <summary>
        /// The Error Message.
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}