using GiG.Core.Context.Abstractions;
using GiG.Core.DistributedTracing.Abstractions;
using GiG.Core.Hosting.Abstractions;
using GiG.Core.Hosting.Extensions;
using GiG.Core.Logging.Enrichers.ApplicationMetadata.Extensions;
using GiG.Core.Logging.Enrichers.Context.Extensions;
using GiG.Core.Logging.Enrichers.DistributedTracing.Extensions;
using GiG.Core.Logging.Enrichers.MultiTenant.Extensions;
using GiG.Core.Logging.Extensions;
using GiG.Core.Logging.Sinks.File.Extensions;
using GiG.Core.Logging.Tests.Integration.Extensions;
using GiG.Core.Logging.Tests.Integration.Helpers;
using GiG.Core.Web.Mock.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GiG.Core.Logging.Tests.Integration.Tests
{
    [Trait("Category", "Integration")]
    public partial class LoggingTests : IAsyncLifetime
    {
        private readonly string _logMessageTest = Guid.NewGuid().ToString();

        private IHost _host;
        private string _filePath;
        private IApplicationMetadataAccessor _applicationMetadataAccessor;
        private IRequestContextAccessor _requestContextAccessor;
        private ICorrelationContextAccessor _correlationContextAccessor;
        private IActivityContextAccessor _activityContextAccessor;
        private SemaphoreSlim _semaphore;

        private LogEvent _logEvent;

        public async Task InitializeAsync()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureServices(x =>
                {
                    x.AddMockCorrelationAccessor();
                    x.AddMockTenantAccessor();
                    x.AddMockRequestContextAccessor();
                    x.AddMockActivityContextAccessor();
                })
                .UseApplicationMetadata()
                .ConfigureLogging(x => x
                    .WriteToFile()
                    .WriteToSink(new DelegatingSink(WriteLog))
                    .EnrichWithApplicationMetadata()
                    .EnrichWithActivityContext()
                    .EnrichWithCorrelation()
                    .EnrichWithTenant()
                    .EnrichWithRequestContext()
                )
                .Build();

            var configuration = _host.Services.GetRequiredService<IConfiguration>();
            _filePath = configuration["Logging:Sinks:File:FilePath"];

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            
            await _host.StartAsync();
            
            _semaphore = new SemaphoreSlim(0, 1);

            _applicationMetadataAccessor = _host.Services.GetRequiredService<IApplicationMetadataAccessor>();
            _requestContextAccessor = _host.Services.GetRequiredService<IRequestContextAccessor>();
            _correlationContextAccessor = _host.Services.GetRequiredService<ICorrelationContextAccessor>();
            _activityContextAccessor = _host.Services.GetRequiredService<IActivityContextAccessor>();
        }

       private void WriteLog(LogEvent log)
        {
            if (log.MessageTemplate.Text != _logMessageTest)
            {
                return;
            }

            _logEvent = log;
            _semaphore.Release();
        }

        private async Task AssertLogEventAsync()
        {
            await _semaphore.WaitAsync(10_000);

            Assert.NotNull(_logEvent);
        }

        public async Task DisposeAsync()
        {
            await _host.StopAsync();
            _host.Dispose();
            _semaphore.Dispose();
            File.Delete(_filePath);
        }
    }
}