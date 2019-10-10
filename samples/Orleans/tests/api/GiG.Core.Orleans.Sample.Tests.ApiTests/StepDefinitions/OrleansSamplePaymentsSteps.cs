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

        [Given(@"I (?:'(Successfully|Unsuccessfully)' )?Deposit '(.*)' on the account for player with IP '(.*)'")]
        public void GivenIDepositOnTheAccountForPlayerWithIP(string depositState, decimal depositAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void DepositOperation()
            {
                Response<decimal> response = _orleansSamplePaymentsService.DepositAsync(new TransactionRequest {Amount = depositAmount})
                    .GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Deposit.ToString(), response);
            }

            if(depositState.Equals("Unsuccessfully"))
                DepositOperation();
            else
                _scenarioContext.Add(SampleApiEndpointKeys.DepositBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, DepositOperation).GetAwaiter().GetResult());
            
        }

        [When(@"I (?:'(Successfully|Unsuccessfully)' )?withdraw '(.*)' from account for player with IP '(.*)'")]
        [Then(@"I (?:'(Successfully|Unsuccessfully)' )?withdraw '(.*)' from account for player with IP '(.*)'")]
        public void WhenIWithdrawFromAccountForPlayerWithIP(string depositState, decimal withdrawalAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void WithdrawOperation()
            {
                Response<decimal> response = _orleansSamplePaymentsService.WithdrawAsync(new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Withdraw.ToString(), response);
            }

            if (depositState.Equals("Unsuccessfully"))
                WithdrawOperation();
            else
                _scenarioContext.Add(SampleApiEndpointKeys.WithdrawalBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, WithdrawOperation).GetAwaiter().GetResult());
        }

        [Then(@"the status code for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheStatusCodeForIs(string operationType, string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(operationType).ResponseMessage.StatusCode.ToString());
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
