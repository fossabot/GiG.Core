using GiG.Core.Orleans.Clustering.Abstractions;

namespace GiG.Core.Orleans.Clustering.Kubernetes.Abstractions
{
    /// <summary>
    /// Kubernetes Options.
    /// </summary>
    public abstract class KubernetesOptions
    {
        /// <summary>
        /// The configuration default section name.
        /// </summary>
        public const string DefaultSectionName = MembershipProviderOptions.DefaultSectionName;

        /// <summary>
        /// The Custom Resource Definition Group name.
        /// </summary>
        public string Group { get; set; } = "orleans.dot.net";

        /// <summary>
        /// The K8s API Endpoint Base Url.
        /// </summary>
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// The Api Token.
        /// </summary>
        public string ApiToken { get; set; }

        /// <summary>
        /// The Certificate Data.
        /// </summary>
        public string CertificateData { get; set; }
    }
}