using System;
using RestEase;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests.Services
{
    public interface IOrleansSampleCommonService
    {
        [Header("player-id")]
        Guid PlayerId { get; set; }

        [Header("X-Forwarded-For")]
        string IPAddress { get; set; }
    }
}
