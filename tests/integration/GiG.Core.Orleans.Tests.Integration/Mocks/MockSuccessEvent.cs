using System;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockSuccessEvent
    {
        public const string MockSuccessEventNamespace = "MockSuccessEventNamespace";
        public Guid GrainId { get; set; }
    }
}