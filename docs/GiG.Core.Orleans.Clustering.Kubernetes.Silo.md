# GiG.Core.Orleans.Clustering.Kubernetes.Silo

This Library provides an API to register an Orleans Silo running on Kubernetes.

## Basic Usage

Add the below to your Startup class and this will register an Orleans Silo running on Kubernetes.

```csharp

        public static void Main()
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices(services => services.AddCorrelationAccessor())
                .UseOrleans(ConfigureOrleans)
                .Build()
                .Run();
        }

        private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)
                .ConfigureDashboard(ctx.Configuration)
                .ConfigureEndpoints()
                .ConfigureKubernetesClustering(ctx.Configuration)
                .AddAssemblies(typeof(Grain));
        }
        
```

### Configuration

You can change the default value for the Kubernetes configuration by overriding the [KubernetesOptions](..\src\GiG.Core.Orleans.Clustering.Kubernetes.Silo\Configurations\KubernetesSiloOptions.cs) by adding the following configuration settings under section `Orleans:Kubernetes`.

| Configuration Name  | Type   | Optional | Default Value                                                     |
|:--------------------|:-------|:---------|:------------------------------------------------------------------|
| Group               | String | Yes      | `orleans.dot.net`                                                 |
| ApiEndpoint         | String | Yes      | Populated from environmental variables when hosted inside the pod |
| ApiToken            | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CertificateData     | String | Yes      | Populated from environmental variables when hosted inside the pod |
| CanCreateResources  | String | Yes      | `false`                                                           |
| DropResourcesOnInit | String | Yes      | `false`                                                           |
