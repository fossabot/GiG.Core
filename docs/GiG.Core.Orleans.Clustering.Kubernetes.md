# GiG.Core.Orleans.Clustering.Kubernetes

This Library provides APIs to register Orleans Clients and Silos running on Kubernetes.

## Basic Usage

### Registering an Orleans Client

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
		}
```

### Registering an Orleans Silo

Add the below to your Program.cs and this will register an Orleans Silo running on Kubernetes.

```csharp

	public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)                                
                .ConfigureServices(Startup.ConfigureServices)
                .UseOrleans(ConfigureOrleans);

        private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)               
                .ConfigureEndpoints()
                .ConfigureKubernetesClustering(ctx.Configuration)
                .AddAssemblies(typeof(Grain));
        }
    } 

```

### Configuration

#### Silo

You can change the default value for the Kubernetes configuration by overriding the [KubernetesSiloOptions](..\src\GiG.Core.Orleans.Clustering.Kubernetes\Configurations\KubernetesSiloOptions.cs) by adding the following configuration settings under section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CanCreateResources  | String | Yes      | `false`                                                           |
| DropResourcesOnInit | String | Yes      | `false`                                                           |

#### Client

You can change the default value for the Kubernetes configuration by overriding the [KubernetesClientOptions](..\src\GiG.Core.Orleans.Clustering.Kubernetes\Configurations\KubernetesClientOptions.cs) by adding the following configuration settings under section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
