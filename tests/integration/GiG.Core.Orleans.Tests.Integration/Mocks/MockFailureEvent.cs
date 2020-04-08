using GiG.Core.Orleans.Streams.Abstractions.Models;
using System;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockFailureEvent : FailedEventBase
    {
        public MockFailureEvent() : base(MockErrorCode)
        {

        }

        public const string MockFailureEventNamespace = "MockFailureEventNamespace";
        
        public const string MockErrorCode = "MockErrorCode";
        
        public const string MockErrorMessage = "MockErrorMessage";
        
        public Guid GrainId { get; set; }
    }
}