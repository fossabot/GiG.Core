﻿namespace GiG.Core.Data.KVStores.Providers.Etcd.Abstractions
{
    /// <summary>
    /// Etcd Provider Options.
    /// </summary>
    public class EtcdProviderOptions
    {
        /// <summary>
        /// The Connection String.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The Key.
        /// </summary>
        public string Key { get; set; }
    }
}