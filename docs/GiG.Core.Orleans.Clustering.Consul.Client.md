# GiG.Core.Orleans.Clustering.Consul.Client

This Library provides an API to register an Orleans Client running on Consul.

## Basic Usage

Add the below to your Startup class and this will register an Orleans Client running on Consul.

```csharp

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddClusterClient((x, sp) =>
            {
                x.AddCorrelationOutgoingFilter(sp);
                x.ConfigureCluster(_configuration);
                x.ConfigureConsulClustering(_configuration);
                x.AddAssemblies(typeof(ITransactionGrain));
            });

```

### Configuration

You can change the default value for the Consul configuration by overriding the [ConsulOptions](..\src\GiG.Core.Orleans.Clustering.Consul.Client\Configurations\ConsulOptions.cs) by adding the following configuration settings under section `Orleans:Consul`.

| Configuration Name | Type   | Optional | Default Value            |
|:-------------------|:-------|:---------|:-------------------------|
| Address            | String | Yes      | `http://localhost:8500"` |
| KvRootFolder       | String | Yes      | `dev`                    |
