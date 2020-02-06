namespace GiG.Core.DistributedTracing.Abstractions
{
    /// <summary>
    /// Constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The Header for Correlation ID.
        /// </summary>
        public const string Header = "X-Correlation-ID";

        /// <summary>
        /// The Header for Activity Trace Parent.
        /// </summary>
        public const string ActivityHeader = "Trace-Parent";
    }
}