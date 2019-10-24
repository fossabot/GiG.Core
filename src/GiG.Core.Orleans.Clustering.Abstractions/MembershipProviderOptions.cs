namespace GiG.Core.Orleans.Clustering.Abstractions
{
    /// <summary>
    /// Membership Provider Options.
    /// </summary>
    public class MembershipProviderOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:MembershipProvider";

        /// <summary>
        /// The Membership Provider name.
        /// </summary>
        public string Name { get; set; }
    }
}