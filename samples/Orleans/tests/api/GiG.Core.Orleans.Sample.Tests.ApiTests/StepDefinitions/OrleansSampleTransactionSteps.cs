using System;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using Xunit;
using RestEase;
using TechTalk.SpecFlow;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    internal class OrleansSampleTransactionSteps
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


        [Given(@"I Deposit '(.*)' on the account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void GivenIDepositOnTheAccountForPlayerWithIdAndIP(decimal depositAmount, string playerState, string ipAddress)
        {
            _orleansSampleTransactionService.SetHeaders(GetPlayerId(playerState), ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.DepositAsync(new TransactionRequest {Amount = depositAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add("Deposit", response);
        }

        [When(@"I request the balance of account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void WhenIRequestTheBalanceOfAccountForPlayerWithIdAndIP(string playerState, string ipAddress)
        {
            _orleansSampleTransactionService.SetHeaders(GetPlayerId(playerState), ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.GetBalanceAsync().GetAwaiter().GetResult();
            _scenarioContext.Add("GetBalance", response);
        }

        [When(@"I withdraw '(.*)' from account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void WhenIWithdrawFromAccountForPlayerWithIdAndIP(decimal withdrawalAmount, string playerState, string ipAddress)
        {
            _orleansSampleTransactionService.SetHeaders(GetPlayerId(playerState), ipAddress);

            Response<decimal> response = _orleansSampleTransactionService.WithdrawAsync(new TransactionRequest {Amount = withdrawalAmount}).GetAwaiter().GetResult();
            _scenarioContext.Add("Withdraw", response);
        }

        [Then(@"the status code for '(GetBalance|Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheStatusCodeForIs(string operationType, string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<decimal>>(operationType).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the '(GetBalance|Deposit|Withdraw)' balance is '(.*)'")]
        public void ThenTheBalanceIs(string operationType, decimal balance)
        {
            
            Assert.Equal(balance, _scenarioContext.Get<Response<decimal>>(operationType).GetContent());
        }

        [Then(@"the error message for '(GetBalance|Deposit|Withdraw)' is '(.*)'")]
        public void ThenTheErrorMessageUsingKeyIs(string operationType, string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<decimal>>(operationType).StringContent);
        }

        private string GetPlayerId(string playerState)
        {
            if (string.IsNullOrEmpty(playerState))
                return _random;
            return playerState.Equals("invalid") ? "!!" : null;
        }
    }
}