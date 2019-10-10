namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestSettings
    {
        public static string BaseUrl()
        {
            return "http://localhost:7000/api/";
        }

        public static string NotificationsUrl()
        {
            return "http://localhost:7000/notifications/open/";
        }
    }
}
