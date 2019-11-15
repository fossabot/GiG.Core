namespace GiG.Core.Hosting.Abstractions
{
    /// <summary>
    /// Info Management Options
    /// </summary>
    public class InfoManagementOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = "InfoManagement";

        /// <summary>
        /// The InfoManagement Url.
        /// </summary>
        public string Url { get; set; } = "/actuator/info";
    }
}