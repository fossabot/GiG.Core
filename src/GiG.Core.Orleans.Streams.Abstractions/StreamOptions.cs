namespace GiG.Core.Orleans.Streams.Abstractions
{
    /// <summary>
    /// Settings for Orleans Streams.
    /// </summary>
    public class StreamOptions
    {
        /// <summary>
        /// Default Section Name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Streams";

        /// <summary>
        /// The Namespace Prefix.
        /// </summary>
        public string NamespacePrefix { get; set; }
    }
}