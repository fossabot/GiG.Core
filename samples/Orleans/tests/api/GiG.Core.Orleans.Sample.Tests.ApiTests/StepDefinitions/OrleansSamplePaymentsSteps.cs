using GiG.Core.Orleans.Sample.Tests.ApiTests.Contracts;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using RestEase;
using System;
using TechTalk.SpecFlow;
using Xunit;

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

        [Given(@"I '(Successfully|Unsuccessfully)' Deposit '(.*)' on the account for player with IP '(.*)'")]
        public void GivenIDepositOnTheAccountForPlayerWithIP(DepositState depositState, decimal depositAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void DepositOperation()
            {
                var response = _orleansSamplePaymentsService.DepositAsync(new TransactionRequest {Amount = depositAmount})
                    .GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Deposit.ToString(), response);
            }

            switch (depositState)
            {
                case DepositState.Unsuccessfully:
                    DepositOperation();
                    break;
                case DepositState.Successfully:
                    _scenarioContext.Add(SampleApiEndpointKeys.DepositBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, DepositOperation).GetAwaiter().GetResult());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(depositState), depositState, null);
            }
            
        }

        [When(@"I '(Successfully|Unsuccessfully)' withdraw '(.*)' from account for player with IP '(.*)'")]
        [Then(@"I '(Successfully|Unsuccessfully)' withdraw '(.*)' from account for player with IP '(.*)'")]
        public void WhenIWithdrawFromAccountForPlayerWithIP(DepositState depositState, decimal withdrawalAmount, string ipAddress)
        {
            _orleansSamplePaymentsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            void WithdrawOperation()
            {
                var response = _orleansSamplePaymentsService.WithdrawAsync(new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
                _scenarioContext.Add(SampleApiEndpointKeys.Withdraw.ToString(), response);
            }

            switch (depositState)
            {
                case DepositState.Unsuccessfully:
                    WithdrawOperation();
                    break;
                case DepositState.Successfully:
                    _scenarioContext.Add(SampleApiEndpointKeys.WithdrawalBalance.ToString(), _sampleApiTestsFixture.GetPlayerBalanceNotification(_sampleApiTestsFixture.PlayerId, WithdrawOperation).GetAwaiter().GetResult());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(depositState), depositState, null);
            }
        }

        [Then(@"the status code for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheStatusCodeForIs(SampleApiEndpointKeys endpointKey, string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(endpointKey.ToString()).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the '(DepositBalance|WithdrawalBalance)' is '(.*)'")]
        public void ThenTheNotifiedBalanceIs(SampleApiEndpointKeys endpointKey, decimal balanceChange)
        {
            Assert.Equal(balanceChange, _scenarioContext.Get<decimal>(endpointKey.ToString()));
        }

        [Then(@"the error message for '(Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheErrorMessageUsingKeyIs(SampleApiEndpointKeys endpointKey, string message)
        {
            var errorMessage = _sampleApiTestsFixture.ParseErrorMessage(_scenarioContext.Get<Response<decimal>>(endpointKey.ToString()).StringContent);
            Assert.Equal(message, errorMessage);
        }
       
    }
}
