# GiG.Core.ApplicationMetrics.Prometheus

This Library provides an API to register application metrics which can be consumed by Prometheus.

## Basic Usage

The below code needs to be added to the `Startup.cs`.  This will register the configuration options and a Metrics Endpoint which can be used by Prometheus. It is important to add `app.UseHttpApplicationMetrics` after `app.UseRouting`.
 
```csharp
	
public static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
{
    var configuration = hostContext.Configuration;
    
    //Application Metrics
    services
        .ConfigureApplicationMetrics(configuration);
}

public void Configure(IApplicationBuilder app)
{   
    app.UseRouting();   
    app.UseHttpApplicationMetrics(); 

    app.UseEndpoints(endpoints => {           
        endpoints.MapApplicationMetrics();
    });
}
```

### Configuration

The below table outlines the valid Configurations used to configure the [ApplicationMetricsOptions](..\src\GiG.Core.ApplicationMetrics.Abstractions\ApplicationMetricsOptions.cs).

| Configuration Name | Type    | Optional | Default Value            |
|:-------------------|:--------|:---------|:-------------------------|
| Url                | String  | No       | `/metrics`               |
| IsEnabled          | Boolean | Yes      | true                    |