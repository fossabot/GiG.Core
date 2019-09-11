namespace GiG.Core.Hosting.Abstractions
{
    public class InfoManagementOptions
    {
        public const string DefaultSectionName = "InfoManagement";

        public bool IsEnabled { get; set; } = true;

        public string Url { get; set; } = "/actuator/info";
    }
}