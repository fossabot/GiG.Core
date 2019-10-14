using Microsoft.Extensions.Configuration;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestSettings
    {
        public static string ApiUrl()
        {
            IConfiguration config = GetConfiguration();
            return config["TestConfiguration:BaseUrl"]+ config["TestConfiguration:ApiPath"];
        }

        public static string NotificationsUrl()
        {
            IConfiguration config = GetConfiguration();
            return config["TestConfiguration:BaseUrl"] + config["TestConfiguration:NotificationsPath"];
        }

        private static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
