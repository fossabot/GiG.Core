using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace GiG.Core.Web.Tests.Component.StepDefinitions
{
    [Binding]
    public class WebSampleSteps
    {
        public WebSampleSteps(ScenarioContext scenarioContext) 
        { 

        }


        [Given(@"I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredIntoTheCalculator(int p0)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
