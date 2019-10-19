namespace GiG.Core.Hosting.Abstractions
{
    /// <summary>
    /// Info Management Options
    /// </summary>
    public class InfoManagementOptions
    {
        /// <summary>
        /// Info Management default section name.
        /// </summary>
        public const string DefaultSectionName = "InfoManagement";

        /// <summary>
        /// Indicates whether InfoManagement is enabled or not.
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// The InfoManagement Url.
        /// </summary>
        public string Url { get; set; } = "/actuator/info";
    }
}