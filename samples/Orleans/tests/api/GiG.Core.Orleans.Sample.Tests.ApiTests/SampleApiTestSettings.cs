using Microsoft.Extensions.Configuration;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestSettings
    {
        public static string ApiUrl()
        {
            var config = GetConfiguration();
            return $"{config.BaseUrl}{config.ApiPath}{config.ApiVersion}";
        }

        public static string NotificationsUrl()
        {
            var config = GetConfiguration();
            return config.BaseUrl + config.NotificationsPath;
        }

        private static TestConfigurationOptions GetConfiguration()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration.GetSection(TestConfigurationOptions.DefaultSectionName).Get<TestConfigurationOptions>();
        }
    }
}
