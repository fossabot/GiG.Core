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

        [Given(@"I Deposit (.*) on the account for player with id (.*) and IP (.*) using key (.*)")]
        public void GivenIDepositOnTheAccountForPlayerWithIdAndIPUsingKey(decimal depositAmount, string playerId, string ipAddress, string key)
        {
            _orleansSampleTransactionService.SetHeaders(playerId + _random, ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.DepositAsync(new TransactionRequest {Amount = depositAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add(key, response);
        }

        [When(@"I request the balance of account for player with id (.*) and IP (.*) using key (.*)")]
        public void WhenIRequestTheBalanceOfAccountForPlayerWithIdAndIPUsingKey(string playerId, string ipAddress, string key)
        {
            _orleansSampleTransactionService.SetHeaders(playerId + _random, ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.GetBalanceAsync().GetAwaiter().GetResult();
            _scenarioContext.Add(key, response);
        }

        [When(@"I withdraw (.*) from account for player with id (.*) and IP (.*) using key (.*)")]
        public void WhenIWithdrawFromAccountForPlayerWithIdAndIPUsingKey(decimal withdrawalAmount, string playerId, string ipAddress, string key)
        {
            _orleansSampleTransactionService.SetHeaders(playerId + _random, ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.WithdrawAsync(new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add(key, response);
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