namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Read
{
    public class EtcdConfiguration
    {
        public string ConnectionString { get; set; } = "http://etcd:2379";
        public int Port { get; set; } = 2379;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CaCertificate { get; set; } = string.Empty;
        public string ClientCertificate { get; set; } = string.Empty;
        public string ClientKey { get; set; } = string.Empty;
        public bool IsPublicRootCa { get; set; } = false;
    }
}