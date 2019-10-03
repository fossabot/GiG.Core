using GiG.Core.Web.Sample.Tests.Component.Services;
using NUnit.Framework;
using RestEase;
using TechTalk.SpecFlow;

namespace GiG.Core.Web.Sample.Tests.Component.StepDefinitions
{
    [Binding]
    public class WebSampleSteps
    {
        private readonly WebSampleService _webSampleService;
        private readonly ScenarioContext _scenarioContext;

        public WebSampleSteps(WebSampleService webSampleService, ScenarioContext scenarioContext) 
        {
            _webSampleService = webSampleService;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I get the current balance of the player")]
        public void GivenIGetTheCurrentBalanceOfThePlayer()
        {
            var balance = _webSampleService.GetBalance();
            _scenarioContext.Add("CurrentBalance", balance);
        }

        [Given(@"I get the minimum deposit limit amount")]
        public void GivenIGetTheMinimumDepositLimitAmount()
        {
            var minimumDepositAmount = _webSampleService.GetMinimumDepositLimit();
            _scenarioContext.Add("MinDepositAmount", minimumDepositAmount);
        }

        [Given(@"I deposit '(.*)'")]
        public void GivenIDeposit(decimal depositAmount)
        {
            _scenarioContext.Add("DepositedAmount", depositAmount);

            var transactionRequest = new TransactionRequest { Amount = depositAmount };
            _webSampleService.Deposit(transactionRequest);
        }

        [Given(@"I deposit '(.*)' '(less|more)' than the minimum deposit amount")]
        [When(@"I deposit '(.*)' '(less|more)' than the minimum deposit amount")]
        public void GivenIDepositMoreThanTheMinimumDepositAmount(decimal depositAmount, string lessOrMore)
        {
            if (lessOrMore.Equals("less")) 
            {
                depositAmount *= -1;
            }

            var minimumDepositAmount = _scenarioContext.Get<decimal>("MinDepositAmount");
            depositAmount += minimumDepositAmount;
            _scenarioContext.Add("DepositedAmount", depositAmount);

            var transactionRequest = new TransactionRequest { Amount = depositAmount };
            var depositResponse = _webSampleService.Deposit(transactionRequest);

            _scenarioContext.Add("DepositResponse", depositResponse);

        }

        [Then(@"the deposit response is a '(.*)'")]
        public void ThenTheDepositResponseIsA(string responseError)
        {
            var depositResponse = _scenarioContext.Get<Response<decimal>>("DepositResponse");

            Assert.AreEqual(responseError, depositResponse.ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the withdraw response is a '(.*)'")]
        public void ThenTheWithdrawResponseIsA(string responseError)
        {
            var withdrawResponse = _scenarioContext.Get<Response<decimal>>("WithdrawResponse");

            Assert.AreEqual(responseError, withdrawResponse.ResponseMessage.StatusCode.ToString());
        }

        [When(@"I get the new balance of the player")]
        public void WhenIGetTheNewBalanceOfThePlayer()
        {
            var balance = _webSampleService.GetBalance();
            _scenarioContext.Add("UpdatedBalance", balance);
        }

        [Then(@"the balance should be updated correctly")]
        public void ThenTheBalanceShouldBeUpdatedCorrectly()
        {
            var currBalance = _scenarioContext.Get<decimal>("CurrentBalance");
            var updatedBalance = _scenarioContext.Get<decimal>("UpdatedBalance");

            _scenarioContext.TryGetValue<decimal>("DepositedAmount", out var depositedAmount);
            _scenarioContext.TryGetValue<decimal>("WithdrawnAmount", out var withdrawnAmount);

            Assert.AreEqual(currBalance + depositedAmount - withdrawnAmount, updatedBalance, "Expected amount is not equal to actual amount");
        }

        [When(@"I withdraw '(.*)'")]
        public void WhenIWithdraw(decimal withdrawnAmount)
        {
            _scenarioContext.Add("WithdrawnAmount", withdrawnAmount);

            var transactionRequest = new TransactionRequest { Amount = withdrawnAmount };
            _webSampleService.Withdraw(transactionRequest);
        }

        [When(@"I withdraw '(.*)' '(less|more)' than the current balance")]
        public void WhenIWithdrawThanTheCurrentBalance(decimal withdrawnAmount, string lessOrMore)
        {
            if (lessOrMore.Equals("less"))
            {
                withdrawnAmount *= -1;
            }

            var currentBalance = _scenarioContext.Get<decimal>("CurrentBalance");
            withdrawnAmount += currentBalance;
            _scenarioContext.Add("WithdrawnAmount", withdrawnAmount);

            var transactionRequest = new TransactionRequest { Amount = withdrawnAmount };
            var withdrawnResponse = _webSampleService.Withdraw(transactionRequest);

            _scenarioContext.Add("WithdrawResponse", withdrawnResponse);
        }

    }
}
