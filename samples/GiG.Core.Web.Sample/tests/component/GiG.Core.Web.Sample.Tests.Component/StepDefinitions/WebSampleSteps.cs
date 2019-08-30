using GiG.Core.Web.Sample.Tests.Component.Services;
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

        public WebSampleSteps() 
        {
            
        }

        [Given(@"I get the current balance of the player")]
        public void GivenIGetTheCurrentBalanceOfThePlayer()
        {
            WebSampleService webSampleService = new WebSampleService();

            decimal balance = webSampleService.GetBalance();
        }

        [Given(@"I deposit '(.*)'")]
        public void GivenIDeposit(int p0)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I get the new balance of the player")]
        public void WhenIGetTheNewBalanceOfThePlayer()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"the balance should be updated correctly")]
        public void ThenTheBalanceShouldBeUpdatedCorrectly()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I withdraw '(.*)'")]
        public void WhenIWithdraw(int p0)
        {
            ScenarioContext.Current.Pending();
        }


    }
}
