namespace GiG.Core.Orleans.Clustering
{
    /// <summary>
    /// Membership Provider Options.
    /// </summary>
    public class MembershipProviderOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:MembershipProvider";

        /// <summary>
        /// Membership Provider Name.
        /// </summary>
        public string Name { get; set; }
    }
}
