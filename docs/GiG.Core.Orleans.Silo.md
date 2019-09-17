# GiG.Core.Orleans.Silo

This Library provides an API to register an Orleans Silo in an application.


## Basic Usage

Add the below to your Program class when creating a new HostBuilder

```csharp

        public static void Main()
        {
            new HostBuilder()
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
                .AddAssemblies(typeof(Grain));
        }
```

### Configuration

You can change the default value for the Cluster configuration by overriding the [ClusterOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Core/Configuration/Options/ClusterOptions.cs) by adding the following configuration settings under section `Orleans:Cluster`

| Configuration Name | Type   | Optional | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ClusterId          | String | Yes      | `dev`         |
| ServiceId          | String | Yes      | `dev`         |

You can change the default values for the Orleans Dashboard configuration by overriding the [DashboardOptions](..\GiG.Core.Orleans.Abstractions\Configuration\DashboardOptions.cs) by adding the following configuration settings under section `Dashboard`

| Configuration Name | Type    | Optional | Default Value |
|:-------------------|:--------|:---------|:--------------|
| Enabled            | Boolean | No       | `false`       |
| Port               | String  | No       | `8181`        |
| Path               | String  | Yes      |               |
