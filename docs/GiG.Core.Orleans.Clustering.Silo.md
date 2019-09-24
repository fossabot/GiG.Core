# GiG.Core.Orleans.Clustering.Silo

This Library provides an API to register a membership provider for Orleans Silo using configuration.

## Basic Usage

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

