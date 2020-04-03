namespace GiG.Core.Orleans.Tests.Integration.Contracts
{
    public class ActivityResponse
    {
        public string TraceId { get; set; }
        
        public string ParentId { get; set; }

        public string RootId { get; set; }

        public string Baggage { get; set; }

        public string TenantId { get; set; }
    }
}
