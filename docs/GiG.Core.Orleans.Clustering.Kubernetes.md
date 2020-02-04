# GiG.Core.Orleans.Clustering.Kubernetes

This Library provides an API to use Kubernetes as a Membership Provider for Orleans Silos.

## Basic Usage

### Registering an Orleans Client

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client running on Kubernetes.
**Note**: The `AddDefaultClusterClient` and `ConfigureCluster` extensions can be found in the nuget package ```GiG.Core.Orleans.Client```

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
**Note**: The `ConfigureCluster` extension can be found in the nuget package ```GiG.Core.Orleans.Silo```
**Note**: The `UseOrleans` extension can be found in the nuget package ```Microsoft.Orleans.Server```

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

The below table outlines the valid Configurations used to override the [KubernetesSiloOptions](../src/GiG.Core.Orleans.Clustering.Kubernetes/Abstractions/KubernetesSiloOptions.cs) under the Config section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CanCreateResources  | String | Yes      | `false`                                                           |
| DropResourcesOnInit | String | Yes      | `false`                                                           |

#### Client

The below table outlines the valid Configurations used to override the [KubernetesClientOptions](../src/GiG.Core.Orleans.Clustering.Kubernetes/Abstractions/KubernetesClientOptions.cs) under the Config section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |

#### Sample Configuration

```json
{
  "Orleans": {
    "MembershipProvider": {
      "Name": "Kubernetes",
      "Group": "orleanstest.dot.net",
      "ApiEndpoint": "https://10.2.31.139:6443/api",
      "ApiToken": "eyJhbGciOiJSUzI1NiIsImtpZCI6IiJ9.eyJpc3MiOiJrdWJlcm5l0Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9uYW1lc3BhY2UiOiJvcmxlYW5zdGVzdCIsImt1YmVybmV0ZXMuaW8vc2VydmljZWFjY291bnQvc2VjcmV0Lm5hbWUiOiJkZWZhdWx0LXRva2VuLWxwZDY3Iiwia3ViZXJuZXRlcy5pby9zZXJ2aWNlYWNjb3VudC9zZXJ2aWNlLWFjY291bnQubmFtZSI6ImRlZmF1bHQiLCJrdWJlcm5ldGVzLmlvL3NlcnZpY2VhY2NvdW50L3NlcnZpY2UtYWNjb3VudC51aWQiOiJhNjUxM2JjNi0wNmU1LTExZWEtYWI5MS1iODI3ZWJmNDhkMTAiLCJzdWIiOiJzeXN0ZW06c2VydmljZWFjY291bnQ6b3JsZWFuc3Rlc3Q6ZGVmYXVsdCJ9.eKLMILeRJdZRSf0S4flIfdM-cfomtc2WHbGnpLtubA-0g1-2QzVfyBxmRDUIIS8jpZs6x5wbP5fxy_ZFPIWHVhbBklqfMWmiix0iQoPi2vdzoIM7ofkKA6PRXahilpDuab2fwDaIOY6BDmJ8_ja-EdyFAjzcfAgZp71dtwmVM5YJJCWdMlqc4ifzjXt_ia6VP-17yG2mqY4-swSmMCulobr7rMHZQVrJlSGXstqwhKJP6KwFHmc52Sg184hECrEZ-kZnfJFIpWbeIX82mj6HDpONFDIBvOphn8hraAaVa3XwI70Zx7hMJza-L7zB7f-ZwK_189naNij-m5AUmMN-dg"
    }
  }
}
```