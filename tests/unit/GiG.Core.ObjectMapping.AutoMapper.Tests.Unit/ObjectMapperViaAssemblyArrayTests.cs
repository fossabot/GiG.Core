using Xunit;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ObjectMapperViaAssemblyArrayTests : ObjectMapperTests, IClassFixture<Fixtures.AutoMapperViaAssemblyArrayFixture>
    {
        public ObjectMapperViaAssemblyArrayTests(Fixtures.AutoMapperViaAssemblyArrayFixture fixture) : base(fixture) { }
    }
}
