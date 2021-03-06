﻿using Xunit;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit
{
    [Trait("Category", "Unit")]
    public class ObjectMapperViaTypeArrayTests : ObjectMapperTests, IClassFixture<Fixtures.AutoMapperViaTypeArrayFixture>
    {
        public ObjectMapperViaTypeArrayTests(Fixtures.AutoMapperViaTypeArrayFixture fixture) : base(fixture) { }
    }
}
