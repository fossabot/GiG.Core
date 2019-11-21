namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class TestConfigurationOptions
    {
        public const string DefaultSectionName = "TestConfiguration";
        public string BaseUrl { get; set; }
        public string ApiPath { get; set; }
        public string ApiVersion { get; set; }
        public string NotificationsPath { get; set; }
    }
}
