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

        public Guid GetPlayerId(string playerState)
        {
            if (string.IsNullOrEmpty(playerState))
                return RandomPlayerId;
            return playerState.Equals("invalid") ? Guid.Parse("!!") : Guid.Empty;
        }
    }
}
