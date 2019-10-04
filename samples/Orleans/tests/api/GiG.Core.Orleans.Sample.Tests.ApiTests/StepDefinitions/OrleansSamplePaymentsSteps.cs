using GiG.Core.Orleans.Sample.Tests.ApiTests.Contracts;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using Xunit;
using RestEase;
using TechTalk.SpecFlow;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    internal class OrleansSamplePaymentsSteps: IClassFixture<SampleApiTestsFixture>
    {
        private readonly OrleansSamplePaymentsService _orleansSamplePaymentsService;
        private readonly ScenarioContext _scenarioContext;
        private readonly SampleApiTestsFixture _sampleApiTestsFixture;

        public OrleansSamplePaymentsSteps(OrleansSamplePaymentsService orleansSamplePaymentsService, ScenarioContext scenarioContext, SampleApiTestsFixture sampleApiTestsFixture)
        {
            _orleansSamplePaymentsService = orleansSamplePaymentsService;
            _scenarioContext = scenarioContext;
            _sampleApiTestsFixture = sampleApiTestsFixture;
        }

        [Given(@"I Deposit '(.*)' on the account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void GivenIDepositOnTheAccountForPlayerWithIdAndIP(decimal depositAmount, string playerState, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.GetPlayerId(playerState), ipAddress);

            Response<decimal> response = _orleansSamplePaymentsService.DepositAsync(new TransactionRequest { Amount = depositAmount }).GetAwaiter().GetResult();
            _scenarioContext.Add("Deposit", response);
        }

        [When(@"I withdraw '(.*)' from account for player with(?: '(.*)' id and)? IP '(.*)'")]
        [Then(@"I withdraw '(.*)' from account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void WhenIWithdrawFromAccountForPlayerWithIdAndIP(decimal withdrawalAmount, string playerState, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.GetPlayerId(playerState), ipAddress);

            Response<decimal> response = _orleansSamplePaymentsService.WithdrawAsync(new TransactionRequest { Amount = withdrawalAmount }).GetAwaiter().GetResult();
            _scenarioContext.Add("Withdraw", response);
        }

        [Then(@"the status code for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheStatusCodeForIs(string operationType, string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(operationType).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the '(Deposit|Withdraw)' balance is '(.*)'")]
        public void ThenTheBalanceIs(string operationType, decimal balance)
        {
            Assert.Equal(balance, _scenarioContext.Get<Response<decimal>>(operationType).GetContent());
        }

        [Then(@"the error message for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheErrorMessageUsingKeyIs(string operationType, string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<decimal>>(operationType).StringContent);
        }
    }
}
