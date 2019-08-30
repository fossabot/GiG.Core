using GiG.Core.Web.Sample.Tests.Component.Services;
using NUnit.Framework;
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

        [Given(@"I deposit '(.*)'")]
        public void GivenIDeposit(decimal depositAmount)
        {
            _scenarioContext.Add("DepositedAmount", depositAmount);

            TransactionRequest transactionRequest = new TransactionRequest { Amount = depositAmount };
            _webSampleService.Deposit(transactionRequest);
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


    }
}
