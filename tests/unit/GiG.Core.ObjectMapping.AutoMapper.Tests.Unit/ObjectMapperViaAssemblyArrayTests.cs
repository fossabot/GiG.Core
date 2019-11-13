using GiG.Core.ObjectMapping.Abstractions;
using GiG.Core.ObjectMapping.AutoMapper.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Xunit;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit
{
    public class ObjectMapperViaAssemblyArrayTests : ObjectMapperTests, IClassFixture<Fixtures.AutoMapperViaAssemblyArrayFixture>
    {
        public ObjectMapperViaAssemblyArrayTests(Fixtures.AutoMapperViaAssemblyArrayFixture fixture) : base(fixture) { }
    }
}
