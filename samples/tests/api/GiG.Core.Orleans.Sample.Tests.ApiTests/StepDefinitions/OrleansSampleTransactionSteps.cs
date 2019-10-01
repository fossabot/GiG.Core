using System;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using Xunit;
using RestEase;
using TechTalk.SpecFlow;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    internal class OrleansSampleTransactionSteps: IClassFixture<OrleansSampleTransactionService>
    {
        private readonly OrleansSampleTransactionService _orleansSampleTransactionService;
        private readonly ScenarioContext _scenarioContext;
        private readonly string _random;

        public OrleansSampleTransactionSteps(OrleansSampleTransactionService orleansSampleTransactionService, ScenarioContext scenarioContext)
        {
            _orleansSampleTransactionService = orleansSampleTransactionService;
            _scenarioContext = scenarioContext;
            _random = new Random().Next(1, 10000).ToString();
        }

        [Given(@"I Deposit (.*) on the account for player with id (.*) and IP (.*)")]
        public void GivenIDepositOnTheAccountForPlayerWithIdAndIP(decimal depositAmount, string playerId, string ipAddress)
        {
            var response = _orleansSampleTransactionService.DepositAsync(playerId + _random,
                ipAddress.Equals("") ? null : ipAddress, new TransactionRequest {Amount = depositAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add("DepositResponse", response);
        }

        [When(@"I request the balance of account for player with id (.*) and IP (.*)")]
        public void WhenIRequestTheBalanceOfAccountForPlayerWithIdAndIP(string playerId, string ipAddress)
        {
            var response =
                _orleansSampleTransactionService.GetBalanceAsync(playerId + _random,
                    ipAddress.Equals("") ? null : ipAddress).GetAwaiter().GetResult();
            _scenarioContext.Add("GetBalanceResponse", response);
        }

        [When(@"I withdraw (.*) from account for player with id (.*) and IP (.*)")]
        public void WhenIWithdrawFromAccountForPlayerWithIdAndIP(decimal withdrawalAmount, string playerId, string ipAddress)
        {
            var response = _orleansSampleTransactionService.WithdrawAsync(playerId + _random,
                ipAddress.Equals("") ? null : ipAddress, new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add("WithdrawResponse", response);
        }

        [Then(@"the status code for key (.*) is (.*)")]
        public void ThenTheStatusCodeForKeyIs(string key, string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(key).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the balance of the account using key (.*) is (.*)")]
        public void ThenTheBalanceOfTheAccountUsingKeyIs(string key, decimal balance)
        {
            Assert.Equal(balance, _scenarioContext.Get<Response<decimal>>(key).GetContent());
        }

        [Then(@"the error message using key (.*) is (.*)")]
        public void ThenTheErrorMessageUsingKeyIs(string key, string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<decimal>>(key).StringContent);
        }

    }
}