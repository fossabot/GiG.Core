using GiG.Core.Web.Sample.Tests.Component.Services;
using NUnit.Framework;
using RestEase;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace GiG.Core.Web.Sample.Tests.Component.StepDefinitions
{
    [Binding]
    public class WebSampleSteps
    {
        public WebSampleService _webSampleService;
        public ScenarioContext _scenarioContext;

        public WebSampleSteps(WebSampleService webSampleService, ScenarioContext scenarioContext) 
        {
            _webSampleService = webSampleService;
            _scenarioContext = scenarioContext;
        }

        [Given(@"I get the current balance of the player")]
        public void GivenIGetTheCurrentBalanceOfThePlayer()
        {
            decimal balance = _webSampleService.GetBalance();
            _scenarioContext.Add("CurrentBalance", balance);
        }

        [Given(@"I get the minimum deposit limit amount")]
        public void GivenIGetTheMinimumDepositLimitAmount()
        {
            decimal minimumDepositAmount = _webSampleService.GetMinimumDepositLimit();
            _scenarioContext.Add("MinDepositAmount", minimumDepositAmount);
        }

        [Given(@"I deposit '(.*)'")]
        public void GivenIDeposit(decimal depositAmount)
        {
            _scenarioContext.Add("DepositedAmount", depositAmount);

            TransactionRequest transactionRequest = new TransactionRequest { Amount = depositAmount };
            _webSampleService.Deposit(transactionRequest);
        }

        [Given(@"I deposit '(.*)' '(less|more)' than the mimimum deposit amount")]
        [When(@"I deposit '(.*)' '(less|more)' than the mimimum deposit amount")]
        public void GivenIDepositMoreThanTheMimimumDepositAmount(decimal depositAmount, string lessOrMore)
        {
            if (lessOrMore.Equals("less")) 
            {
                depositAmount *= -1;
            }

            decimal minimumDepositAmount = _scenarioContext.Get<decimal>("MinDepositAmount");
            depositAmount += minimumDepositAmount;
            _scenarioContext.Add("DepositedAmount", depositAmount);

            TransactionRequest transactionRequest = new TransactionRequest { Amount = depositAmount };
            Response<decimal> depositResponse = _webSampleService.Deposit(transactionRequest);

            _scenarioContext.Add("DepositResponse", depositResponse);

        }

        [Then(@"the deposit response is a '(.*)'")]
        public void ThenTheDepositResponseIsA(string responseError)
        {
            Response<decimal> depositResponse = _scenarioContext.Get<Response<decimal>>("DepositResponse");

            Assert.AreEqual(responseError, depositResponse.ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the withdraw response is a '(.*)'")]
        public void ThenTheWithdrawResponseIsA(string responseError)
        {
            Response<decimal> withdrawResponse = _scenarioContext.Get<Response<decimal>>("WithdrawResponse");

            Assert.AreEqual(responseError, withdrawResponse.ResponseMessage.StatusCode.ToString());
        }

        [When(@"I get the new balance of the player")]
        public void WhenIGetTheNewBalanceOfThePlayer()
        {
            decimal balance = _webSampleService.GetBalance();
            _scenarioContext.Add("UpdatedBalance", balance);
        }

        [Then(@"the balance should be updated correctly")]
        public void ThenTheBalanceShouldBeUpdatedCorrectly()
        {
            decimal currBalance = _scenarioContext.Get<decimal>("CurrentBalance");
            decimal updatedBalance = _scenarioContext.Get<decimal>("UpdatedBalance");

            _scenarioContext.TryGetValue<decimal>("DepositedAmount", out decimal depositedAmount);
            _scenarioContext.TryGetValue<decimal>("WithdrawnAmount", out decimal withdrawnAmount);

            Assert.AreEqual(currBalance + depositedAmount - withdrawnAmount, updatedBalance, "Expected amount is not equal to actual amount");
        }

        [When(@"I withdraw '(.*)'")]
        public void WhenIWithdraw(decimal withdrawnAmount)
        {
            _scenarioContext.Add("WithdrawnAmount", withdrawnAmount);

            TransactionRequest transactionRequest = new TransactionRequest { Amount = withdrawnAmount };
            _webSampleService.Withdraw(transactionRequest);
        }

        [When(@"I withdraw '(.*)' '(less|more)' than the current balance")]
        public void WhenIWithdrawThanTheCurrentBalance(decimal withdrawnAmount, string lessOrMore)
        {
            if (lessOrMore.Equals("less"))
            {
                withdrawnAmount *= -1;
            }

            decimal currentBalance = _scenarioContext.Get<decimal>("CurrentBalance");
            withdrawnAmount += currentBalance;
            _scenarioContext.Add("WithdrawnAmount", withdrawnAmount);

            TransactionRequest transactionRequest = new TransactionRequest { Amount = withdrawnAmount };
            Response<decimal> withdrawnResponse = _webSampleService.Withdraw(transactionRequest);

            _scenarioContext.Add("WithdrawResponse", withdrawnResponse);
        }

    }
}
