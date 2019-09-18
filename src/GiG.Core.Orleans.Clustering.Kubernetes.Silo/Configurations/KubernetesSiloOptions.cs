using System;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Silo.Configurations
{
    /// <summary>
    /// Orleans Kubernetes Settings.
    /// </summary>
    public class KubernetesOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        public const string DefaultSectionName = "Orleans:Kubernetes";

        /// <summary>
        /// Custom Resource Definition Group name.
        /// </summary>
        public string Group { get; set; } = "orleans.dot.net";

        /// <summary>
        /// K8s API Endpoint Base URL.
        /// </summary>
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// K8s API Endpoint Base URL.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Token data
        /// </summary>
        public string CertificateData { get; set; }

        /// <summary>
        /// Create Resources on Initialisation.
        /// </summary>
        public bool CanCreateResources { get; set; } = false;

        /// <summary>
        /// Drop Resources on Initialisation.
        /// </summary>
        public bool DropResourcesOnInit { get; set; } = false;
    }
}
