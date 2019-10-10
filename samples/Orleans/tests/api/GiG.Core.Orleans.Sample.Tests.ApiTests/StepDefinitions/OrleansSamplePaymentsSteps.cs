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

        public OrleansSamplePaymentsSteps(OrleansSamplePaymentsService orleansSamplePaymentsService, ScenarioContext scenarioContext, SampleApiTestsFixture sampleApiTestsFixture, OrleansSampleSignalRHelper orleansSampleSignalRHelper)
        {
            _orleansSamplePaymentsService = orleansSamplePaymentsService;
            _scenarioContext = scenarioContext;
            _sampleApiTestsFixture = sampleApiTestsFixture;
        }

        [Given(@"I Deposit '(.*)' on the account for player with IP '(.*)'")]
        public void GivenIDepositOnTheAccountForPlayerWithIP(decimal depositAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void DepositOperation()
            {
                Response<decimal> response = _orleansSamplePaymentsService.DepositAsync(new TransactionRequest {Amount = depositAmount})
                    .GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Deposit.ToString(), response);
            }

            _scenarioContext.Add(SampleApiEndpointKeys.DepositBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, DepositOperation).GetAwaiter().GetResult());
        }

        [Given(@"I attempt to Deposit '(.*)' on the account for player with IP '(.*)'")]
        public void GivenIAttemptToDepositOnTheAccountForPlayerWithIP(decimal depositAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            Response<decimal> response = _orleansSamplePaymentsService.DepositAsync(new TransactionRequest { Amount = depositAmount })
                .GetAwaiter().GetResult();
            _scenarioContext.Add(SampleApiEndpointKeys.Deposit.ToString(), response);
        }

        [When(@"I withdraw '(.*)' from account for player with IP '(.*)'")]
        [Then(@"I withdraw '(.*)' from account for player with IP '(.*)'")]
        public void WhenIWithdrawFromAccountForPlayerWithIP(decimal withdrawalAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void WithdrawOperation()
            {
                Response<decimal> response = _orleansSamplePaymentsService.WithdrawAsync(new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Withdraw.ToString(), response);
            }

            _scenarioContext.Add(SampleApiEndpointKeys.WithdrawalBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, WithdrawOperation).GetAwaiter().GetResult());
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

        [Then(@"the '(DepositBalance|WithdrawalBalance)' is '(.*)'")]
        public void ThenTheNotifiedBalanceIs(string operationType, decimal balanceChange)
        {
            Assert.Equal(balanceChange, _scenarioContext.Get<decimal>(operationType));
        }

        [Then(@"the error message for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheErrorMessageUsingKeyIs(string operationType, string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<decimal>>(operationType).StringContent);
        }
    }
}
