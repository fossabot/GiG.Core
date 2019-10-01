# GiG.Core.Orleans.Clustering

This Library provides APIs to register membership providers for Orleans Clients and Silos using configuration.

## Basic Usage

### Registering an Orleans Client

Add the below to your Startup class and this will register an Orleans Client running either on Consul or Kubernetes according to the configuration.

```csharp

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClusterClient((x, sp) =>
            {              
                x.ConfigureCluster(_configuration);
                x.UseMembershipProvider(_configuration, builder =>
                {
                    builder.ConfigureConsulClustering(_configuration);
                    builder.ConfigureKubernetesClustering(_configuration);
                });
                x.AddAssemblies(typeof(ITransactionGrain));
            });

```

### Registering an Orleans Silo

Add the below to your Startup class and this will register an Orleans Silo running either on Consul or Kubernetes according to the configuration.

```csharp

        private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
        {
            builder.ConfigureCluster(ctx.Configuration)                
                .ConfigureEndpoints()
                .UseMembershipProvider(ctx.Configuration, x =>
                {
                    x.ConfigureConsulClustering(ctx.Configuration);
                    x.ConfigureKubernetesClustering(ctx.Configuration);
                })
                .AddAssemblies(typeof(Grain));
        }

```

### Configuration

You can change the default value for the membership provider configuration by overriding the [MembershipProviderOptions](..\src\GiG.Core.Orleans.Clustering\MembershipProviderOptions.cs) by adding the following configuration settings under section `Orleans:MembershipProvider`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Name               | String | No       | <null>                   |

