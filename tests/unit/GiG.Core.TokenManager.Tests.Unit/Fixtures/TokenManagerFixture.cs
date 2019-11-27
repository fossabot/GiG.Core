using GiG.Core.Providers.DateTime;
using GiG.Core.TokenManager.Extensions;
using GiG.Core.TokenManager.Implementation;
using GiG.Core.TokenManager.Models;
using GiG.Core.TokenManager.Tests.Unit.Tests;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Net.Http;

namespace GiG.Core.TokenManager.Tests.Unit.Fixtures
{
    public class TokenManagerFixture
    {
        private readonly ServiceProvider _serviceProvider;
        
        internal readonly IHttpClientFactory HttpClientFactory;
        internal TokenClientFactory GetTokenClientFactory() => new TokenClientFactory(HttpClientFactory, new LoggerFactory(), new LocalDateTimeProvider());
        internal ILogger GetLogger() => _serviceProvider.GetRequiredService<ILogger<TokenManagerTests>>();
        
        public TokenManagerFixture()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            
            _serviceProvider = new ServiceCollection()
                .AddTokenManager(config.GetSection("TokenManager"))
                .AddLogging()
                .BuildServiceProvider();
            
            HttpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
        }
    }
}