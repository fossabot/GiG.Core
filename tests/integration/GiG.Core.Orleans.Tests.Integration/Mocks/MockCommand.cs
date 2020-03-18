using System;

namespace GiG.Core.Orleans.Tests.Integration.Mocks
{
    public class MockCommand
    {
        public const string MockCommandNamespace = "MockCommandNamespace";

        public bool IsSuccess { get; set; }
        
        public Guid Id { get; set; }

        public MockCommand(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }
    }
}