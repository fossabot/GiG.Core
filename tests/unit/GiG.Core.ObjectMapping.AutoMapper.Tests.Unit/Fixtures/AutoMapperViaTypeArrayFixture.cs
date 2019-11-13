using GiG.Core.ObjectMapping.Abstractions;
using GiG.Core.ObjectMapping.AutoMapper.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Fixtures
{
    public class AutoMapperViaTypeArrayFixture : AutoMapperFixtureBase
    {
        public AutoMapperViaTypeArrayFixture()
        {
            var host = Host
                .CreateDefaultBuilder()
                .ConfigureServices(x => x.AddObjectMapper(typeof(Profiles.MappingProfile)))
                .Build();

            ObjectMapper = host.Services.GetRequiredService<IObjectMapper>();
        }
    }
}
