using Microsoft.Extensions.Configuration;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestSettings
    {
        public static string ApiUrl()
        {
            TestConfigurationOptions config = GetConfiguration();
            return config.BaseUrl + config.ApiPath;
        }

        public static string NotificationsUrl()
        {
            TestConfigurationOptions config = GetConfiguration();
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
