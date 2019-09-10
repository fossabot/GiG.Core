namespace GiG.Core.Hosting.Abstractions
{
    public class InfoManagementOptions
    {
        public const string DefaultSectionName = "InfoManagement";

        public bool IsEnabled { get; set; }

        public string Url { get; set; } = "/actuator/info";
    }
}