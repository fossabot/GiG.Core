using System;
using System.Collections.Generic;
using System.Linq;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Models;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using Newtonsoft.Json;
using Xunit;
using RestEase;
using TechTalk.SpecFlow;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    internal class OrleansSamplePaymentTransactionsSteps: IClassFixture<SampleApiTestsFixture>
    {
        private readonly OrleansSamplePaymentTransactionsService _orleansSamplePaymentTransactionsService;
        private readonly ScenarioContext _scenarioContext;
        private readonly SampleApiTestsFixture _sampleApiTestsFixture;

        public OrleansSamplePaymentTransactionsSteps(OrleansSamplePaymentTransactionsService orleansSampleTransactionService, ScenarioContext scenarioContext, SampleApiTestsFixture sampleApiTestsFixture)
        {
            _orleansSamplePaymentTransactionsService = orleansSampleTransactionService;
            _scenarioContext = scenarioContext;
            _sampleApiTestsFixture = sampleApiTestsFixture;
        }

        [When(@"I request the transactions of account for player with(?: '(.*)' id and)? IP '(.*)'")]
        public void WhenIRequestTheTransactionsOfAccountForPlayerWithIdAndIP(string playerState, string ipAddress)
        {
            _orleansSamplePaymentTransactionsService.SetHeaders(_sampleApiTestsFixture.GetPlayerId(playerState), ipAddress);

            Response<List<PaymentTransaction>> response = _orleansSamplePaymentTransactionsService.GetPaymentTransactionsAsync().GetAwaiter().GetResult();
            _scenarioContext.Add("GetBalance", response);
        }

        [Then(@"the status code is '(.*)'")]
        public void ThenTheStatusCodeIs(string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<List<PaymentTransaction>>>("GetBalance").ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the number of transactions is '(.*)' with transaction types '(.*)' and values '(.*)'")]
        public void ThenTheNumberOfTransactionsIsWithTransactionTypesAndValues(int numberOfTransactions, string transactionTypes, string balance)
        {
            List<PaymentTransaction> paymentTransactions = DeserializeResponseObject(_scenarioContext.Get<Response<List<PaymentTransaction>>>("GetBalance").StringContent);
            List<string> transactionTypesList = transactionTypes.Split(",").ToList();
            List<string> transactionAmountList = balance.Split(",").ToList();
            
            Assert.Equal(numberOfTransactions, paymentTransactions.Count);

            for (int i = 0; i < numberOfTransactions; i++)
            {
                Assert.Equal(Convert.ToDecimal(transactionAmountList[i]), paymentTransactions[i].Amount);
                Assert.Equal(transactionTypesList[i], paymentTransactions[i].TransactionType.ToString());
            }
        }

        [Then(@"the error message is '(.*)'")]
        public void ThenTheErrorMessageIs(string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<List<PaymentTransaction>>>("GetBalance").StringContent);
        }

        private List<PaymentTransaction> DeserializeResponseObject(string response)
        {
            return JsonConvert.DeserializeObject<List<PaymentTransaction>>(response);
        }
    }
}