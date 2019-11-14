# GiG.Core.Orleans.Clustering.Kubernetes

This Library provides an API to use Kubernetes as a Membership Provider for Orleans Silos.

## Basic Usage

### Registering an Orleans Client

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client running on Kubernetes.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddDefaultClusterClient((x, sp) =>
    {               
        x.ConfigureCluster(_configuration);
        x.ConfigureKubernetesClustering(_configuration);
        x.AddAssemblies(typeof(ITransactionGrain));
    });
}

```

### Registering an Orleans Silo

The below code needs to be added to the `Program.cs`. This will register an Orleans Silo running on Kubernetes.

```csharp

static class Program
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
            .ConfigureEndpoints(ctx.Configuration)
            .ConfigureKubernetesClustering(ctx.Configuration)
            .AddAssemblies(typeof(Grain));
    }
} 

```

### Configuration

#### Silo

The below table outlines the valid Configurations used to override the [KubernetesSiloOptions](..\src\GiG.Core.Orleans.Clustering.Kubernetes\Configurations\KubernetesSiloOptions.cs) under the Config section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CanCreateResources  | String | Yes      | `false`                                                           |
| DropResourcesOnInit | String | Yes      | `false`                                                           |

#### Client

The below table outlines the valid Configurations used to override the [KubernetesClientOptions](..\src\GiG.Core.Orleans.Clustering.Kubernetes\Configurations\KubernetesClientOptions.cs) under the Config section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
