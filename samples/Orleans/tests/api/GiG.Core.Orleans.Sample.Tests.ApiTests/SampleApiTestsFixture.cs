using System;

namespace GiG.Core.Orleans.Sample.Tests.ApiTests
{
    public class SampleApiTestsFixture
    {
        public Guid RandomPlayerId { get; }

        public  SampleApiTestsFixture()
        {
            RandomPlayerId = Guid.NewGuid();
        }
    }
}
