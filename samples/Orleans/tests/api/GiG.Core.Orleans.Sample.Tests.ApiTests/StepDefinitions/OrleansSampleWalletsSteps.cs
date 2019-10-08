using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using RestEase;
using TechTalk.SpecFlow;
using Xunit;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    public sealed class OrleansSampleWalletsSteps: IClassFixture<SampleApiTestsFixture>
    {
        private readonly OrleansSampleWalletsService _orleansSampleWalletsService;
        private readonly ScenarioContext _scenarioContext;
        private readonly SampleApiTestsFixture _sampleApiTestsFixture;

        private readonly string _apiEndpointKey = SampleApiEndpointKeys.WalletBalance.ToString();

        public OrleansSampleWalletsSteps(OrleansSampleWalletsService orleansSampleWalletsService, ScenarioContext scenarioContext, SampleApiTestsFixture sampleApiTestsFixture)
        {
            _orleansSampleWalletsService = orleansSampleWalletsService;
            _scenarioContext = scenarioContext;
            _sampleApiTestsFixture = sampleApiTestsFixture;
        }

        [When(@"I request the wallet balance for player with IP '(.*)'")]
        public void WhenIRequestTheWalletBalanceForPlayerWithIP(string ipAddress)
        {
            _orleansSampleWalletsService.SetHeaders(_sampleApiTestsFixture.RandomPlayerId, ipAddress);
            Response<decimal> response = _orleansSampleWalletsService.GetBalanceAsync().GetAwaiter().GetResult();
            _scenarioContext.Add(_apiEndpointKey, response);
        }

        [Then(@"the status code for get wallet balance is '(.*)'")]
        public void ThenTheStatusCodeForGetWalletBalanceIs(string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(_apiEndpointKey).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the wallet balance is '(.*)'")]
        public void ThenTheWalletBalanceIs(string walletBalance)
        {
            Assert.Equal(walletBalance, _scenarioContext.Get<Response<decimal>>(_apiEndpointKey).StringContent);
        }

        [Then(@"the error message for wallet balance is '(.*)'")]
        public void ThenTheErrorMessageForWalletBalanceIs(string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<decimal>>(_apiEndpointKey).StringContent);
        }

    }
}
