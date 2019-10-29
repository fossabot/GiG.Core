using GiG.Core.Data.KVStores.Abstractions;
using GiG.Core.Data.KVStores.Providers.Hosting;
using GiG.Core.Data.KVStores.Tests.Integration.Mocks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace GiG.Core.Data.KVStores.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public class DataProviderTests
    {
        [Fact]
        public void GetData_JsonDataProvider_ReturnsMockLanguages()
        {
            // Arrange.
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(MockStartup.ConfigureServices)
                .Build();

            var serviceProvider = host.Services;
            
            var providerHostedService = serviceProvider.GetRequiredService<IHostedService>() as ProviderHostedService<IEnumerable<MockLanguage>>;
            
             providerHostedService?.StartAsync(CancellationToken.None);
            
            var dataRetriever = serviceProvider.GetRequiredService<IDataRetriever<IEnumerable<MockLanguage>>>();
            
            // Act.
            var data = dataRetriever.Get();
            
            //Assert.
            Assert.NotNull(data);
        }
    }
}