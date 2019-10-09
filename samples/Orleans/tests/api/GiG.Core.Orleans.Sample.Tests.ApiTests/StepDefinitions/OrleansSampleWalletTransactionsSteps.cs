using System;
using System.Collections.Generic;
using System.Linq;
using GiG.Core.Orleans.Sample.Contracts.Models.Wallet;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Services;
using Newtonsoft.Json;
using RestEase;
using TechTalk.SpecFlow;
using Xunit;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.StepDefinitions
{
    [Binding]
    public sealed class OrleansSampleWalletTransactionsSteps: IClassFixture<SampleApiTestsFixture>
    {
        private readonly OrleansSampleWalletTransactionsService _orleansSampleWalletTransactionsService;
        private readonly ScenarioContext _scenarioContext;
        private readonly SampleApiTestsFixture _sampleApiTestsFixture;

        private readonly string _apiEndpointKey = SampleApiEndpointKeys.WalletTransactions.ToString();

        public OrleansSampleWalletTransactionsSteps(OrleansSampleWalletTransactionsService orleansSampleWalletTransactionsService, ScenarioContext scenarioContext, SampleApiTestsFixture sampleApiTestsFixture)
        {
            _orleansSampleWalletTransactionsService = orleansSampleWalletTransactionsService;
            _scenarioContext = scenarioContext;
            _sampleApiTestsFixture = sampleApiTestsFixture;
        }

        [When(@"I request the wallet transactions of account for player with IP '(.*)'")]
        public void WhenIRequestTheWalletTransactionsOfAccountForPlayerWithIP(string ipAddress)
        {
            _orleansSampleWalletTransactionsService.SetHeaders(_sampleApiTestsFixture.PlayerId, ipAddress);

            Response<List<WalletTransaction>> response = _orleansSampleWalletTransactionsService.GetWalletTransactionsAsync().GetAwaiter().GetResult();
            _scenarioContext.Add(_apiEndpointKey, response);
        }

        [Then(@"the status code for wallet transactions is '(.*)'")]
        public void ThenTheStatusCodeForWalletTransactionsIs(string statusCode)
        {
            Assert.Equal(statusCode, _scenarioContext.Get<Response<List<WalletTransaction>>>(_apiEndpointKey).ResponseMessage.StatusCode.ToString());
        }

        [Then(@"the number of transactions is '(.*)' with transaction types '(.*)', amount '(.*)' and new balance '(.*)'")]
        public void ThenTheNumberOfTransactionsIsWithTransactionTypesAmountAndNewBalance(int numberOfTransactions, string transactionTypes, string amount, string newBalance)
        {
            List<WalletTransaction> walletTransactions = DeserializeResponseObject(_scenarioContext.Get<Response<List<WalletTransaction>>>(_apiEndpointKey).StringContent);
            List<string> transactionTypesList = transactionTypes.Split(",").ToList();
            List<string> amountList = amount.Split(",").ToList();
            List<string> newBalanceList = newBalance.Split(",").ToList();

            Assert.Equal(numberOfTransactions, walletTransactions.Count);

            for (int i = 0; i < numberOfTransactions; i++)
            {
                Assert.Equal(Convert.ToDecimal(amountList[i]), walletTransactions[i].Amount);
                Assert.Equal(Convert.ToDecimal(newBalanceList[i]), walletTransactions[i].NewBalance);
                Assert.Equal(transactionTypesList[i], walletTransactions[i].TransactionType.ToString());
            }
        }

        [Then(@"the error message for wallet transactions is '(.*)'")]
        public void ThenTheErrorMessageForWalletTransactionsIs(string message)
        {
            Assert.Equal(message, _scenarioContext.Get<Response<List<WalletTransaction>>>(_apiEndpointKey).StringContent);
        }

        private List<WalletTransaction> DeserializeResponseObject(string response)
        {
            return JsonConvert.DeserializeObject<List<WalletTransaction>>(response);
        }
    }
}
