# GiG.Core.Orleans.Clustering.Kubernetes.Client

This Library provides an API to register an Orleans Client running on Kubernetes.

## Basic Usage

Add the below to your Startup class and this will register an Orleans Client running on Kubernetes.

```csharp

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClusterClient((x, sp) =>
            {               
                x.ConfigureCluster(_configuration);
                x.ConfigureKubernetesClustering(_configuration);
                x.AddAssemblies(typeof(ITransactionGrain));
            });

```

### Configuration

You can change the default value for the Kubernetes configuration by overriding the [KubernetesOptions](,,\src\GiG.Core.Orleans.Clustering.Kubernetes.Silo\Configurations\KubernetesSiloOptions.cs) by adding the following configuration settings under section `Orleans:Kubernetes`.

| Configuration Name | Type   | Optional | Default Value                                                     |
|:-------------------|:-------|:---------|:------------------------------------------------------------------|
| Group              | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint        | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken           | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData    | String | Yes      | Populated from environmental variables when hosted inside the pod |
