using GiG.Core.ObjectMapping.Abstractions;
using GiG.Core.ObjectMapping.AutoMapper.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Fixtures
{
    public class AutoMapperViaAssemblyArrayFixture : AutoMapperFixtureBase
    {
        public AutoMapperViaAssemblyArrayFixture()
        {
            var host = Host
                .CreateDefaultBuilder()
                .ConfigureServices(x => x.AddObjectMapper(Assembly.GetExecutingAssembly()))
                .Build();

            ObjectMapper = host.Services.GetRequiredService<IObjectMapper>();
        }
    }
}
