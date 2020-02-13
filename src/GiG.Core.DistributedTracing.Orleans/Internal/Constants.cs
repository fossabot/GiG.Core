namespace GiG.Core.DistributedTracing.Orleans.Internal
{
    /// <summary>
    /// Constants.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The Name of the Activity to be started by the Incoming Grain Filter.
        /// </summary>
        public const string IncomingGrainFilterActivityName = "IncomingGrainCall";
    }
}
