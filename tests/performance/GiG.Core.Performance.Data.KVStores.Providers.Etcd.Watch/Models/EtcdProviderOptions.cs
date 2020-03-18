namespace GiG.Core.Performance.Data.KVStores.Providers.Etcd.Watch.Models
{
    /// <summary>
    /// Etcd Provider Options.
    /// </summary>
    public class EtcdProviderOptions
    {
        /// <summary>
        /// The Connection String.
        /// </summary>
        public string ConnectionString { get; set; } = "http://localhost:2379";
        
        /// <summary>
        /// The Etcd Client Port.
        /// </summary>
        public int Port { get; set; } = 2379;
        
        /// <summary>
        /// The Username for etcd basic auth. Default : Empty String.
        /// </summary>
        public string Username { get; set; } = string.Empty;
        
        /// <summary>
        /// The Password for etcd basic auth. Default : Empty String.
        /// </summary>
        public string Password { get; set; } = string.Empty;
        
        /// <summary>
        /// The Certificate Authorities Certificate when using self signed certificates with etcd. Default : Empty String.
        /// </summary>
        public string CaCertificate { get; set; } = string.Empty;
        
        /// <summary>
        /// The Client Certificate when using self signed certificates with client auth enabled in etcd. Default : Empty String.
        /// </summary>
        public string ClientCertificate { get; set; } = string.Empty;
        
        /// <summary>
        ///  The Client Key when using self signed certificates with client auth enabled in etcd. Default : Empty String.
        /// </summary>
        public string ClientKey { get; set; } = string.Empty;
        
        /// <summary>
        ///  Bool depicting whether to use publicly trusted roots to connect to etcd. Default : false.
        /// </summary>
        public bool IsPublicRootCa { get; set; } = false;

        /// <summary>
        /// The Key.
        /// </summary>
        public string Key { get; set; }
    }
}