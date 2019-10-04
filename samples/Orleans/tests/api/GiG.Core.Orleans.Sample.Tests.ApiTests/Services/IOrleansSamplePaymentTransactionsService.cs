using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GiG.Core.Orleans.Sample.Tests.ApiTests.Models;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    [AllowAnyStatusCode]
    public interface IOrleansSamplePaymentTransactionsService
    {
        [Header("player-id")]
        Guid PlayerId { get; set; }

        [Header("X-Forwarded-For")]
        string IPAddress { get; set; }

        [Get("PaymentTransactions")]
        Task<Response<List<PaymentTransaction>>> GetPaymentTransactionsAsync();
    }
}
