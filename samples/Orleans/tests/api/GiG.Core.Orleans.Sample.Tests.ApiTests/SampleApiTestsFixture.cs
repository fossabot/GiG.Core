using Bogus;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestsFixture
    {
        public Guid PlayerId { get; }

        public  SampleApiTestsFixture()
        {
            PlayerId = new Faker().Random.Guid();
        }

        public async Task<decimal> GetPlayerBalanceNotification(Guid playerId, Action operation)
        {
            return await new OrleansSampleSignalRHelper().ListenForNotification("BalanceChanged", "SubscribeAsync", playerId.ToString(), operation);
        }
        
        public string ParseErrorMessage(string responseContent)
        {
            string errorMessage;
            try
            {
                var response = JsonConvert.DeserializeObject<JObject>(responseContent);
                errorMessage = response.SelectToken("$.errors").Values().FirstOrDefault().AsJEnumerable().FirstOrDefault()?.ToString();
            }
            catch (JsonReaderException)
            {
                errorMessage = responseContent;
            }

            return errorMessage;
        }
    }
}
