namespace GiG.Core.Orleans.Clustering.Kubernetes.Configurations
{
    /// <summary>
    /// Orleans Kubernetes Settings.
    /// </summary>
    public class KubernetesSiloOptions : KubernetesOptions
    {
        /// <summary>
        /// Create Resources on Initialization.
        /// </summary>
        /// <remarks>
        /// The default value is <c>false</c>
        /// </remarks>
        public bool CanCreateResources { get; set; } = false;

        /// <summary>
        /// Drop Resources on Initialization.
        /// </summary>
        /// <remarks>
        /// The default value is <c>false</c>
        /// </remarks>
        public bool DropResourcesOnInit { get; set; } = false;
    }
}
