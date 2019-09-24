# GiG.Core.Orleans.Clustering.Client

This Library provides an API to register a membership provider for Orleans Client using configuration.

## Basic Usage

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

### Configuration

You can change the default value for the membership provider configuration by overriding the [MembershipProviderOptions](..\src\GiG.Core.Orleans.Clustering\MembershipProviderOptions.cs) by adding the following configuration settings under section `Orleans:MembershipProvider`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Name               | String | No       | <null>                   |

