namespace GiG.Core.Orleans.Clustering.Kubernetes
{
    /// <summary>
    /// Kubernetes Silo Options.
    /// </summary>
    public class KubernetesSiloOptions : KubernetesOptions
    {
        /// <summary>
        /// A value to indicate if the resources can be created on initialization or not.
        /// </summary>
        public bool CanCreateResources { get; set; } = false;

        /// <summary>
        /// A value to indicate if the resources are dropped on initialization or not.
        /// </summary>
        public bool DropResourcesOnInit { get; set; } = false;
    }
}