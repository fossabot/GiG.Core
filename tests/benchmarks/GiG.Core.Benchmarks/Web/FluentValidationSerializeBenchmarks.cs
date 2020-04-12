using BenchmarkDotNet.Attributes;
using GiG.Core.Validation.FluentValidation.Web.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiG.Core.Benchmarks.Web
{
    [BenchmarkCategory("Validations")]
    public class FluentValidationSerializeBenchmarks
    {
        private HttpClient _client;
        private IHost _host;

        [GlobalSetup]
        public async Task Setup()
        {
            _host = Host.CreateDefaultBuilder()
                .ConfigureLogging(x => x.SetMinimumLevel(LogLevel.Warning))
                .ConfigureServices(x =>
                    x.AddControllers().AddApplicationPart(typeof(ValidationController).Assembly))
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseTestServer();
                    x.Configure(app =>
                    {
                        app.UseRouting();
                        app.UseFluentValidationMiddleware();
                        app.UseEndpoints(builder => builder.MapControllers());
                    });
                })
                .Build();

            await _host.StartAsync();

            _client = _host.GetTestClient();
        } 

        [Benchmark]
        public async Task Serialize_SmallResponse()
        {
            await _client.GetAsync("validation/small");
        }

        [Benchmark]
        public async Task Serialize_LargeResponse()
        {
            await _client.GetAsync("validation/large");
        }

        [GlobalCleanup]
        public async Task Cleanup()
        {
            _client.Dispose();
            await _host.StopAsync();
            _host.Dispose();
        } 
    }
}