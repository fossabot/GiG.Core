namespace GiG.Core.Orleans.Clustering.Kubernetes.Client.Configurations
{
    /// <summary>
    /// Orleans Kubernetes Settings.
    /// </summary>
    public class KubernetesOptions
    {
        /// <summary>
        /// Default configuration section name.
        /// </summary>
        /// <remarks>
        /// The default value is <c>"Orleans:Kubernetes"</c>
        /// </remarks>
        public const string DefaultSectionName = "Orleans:Kubernetes";

        /// <summary>
        /// Custom Resource Definition Group name.
        /// </summary>
        /// <remarks>
        /// The default value is <c>"orleans.dot.net"</c>
        /// </remarks>
        public string Group { get; set; } = "orleans.dot.net";

        /// <summary>
        /// K8s API Endpoint Base URL.
        /// </summary>
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// Api Token.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// Certificate Data.
        /// </summary>
        public string CertificateData { get; set; }
    }
}
