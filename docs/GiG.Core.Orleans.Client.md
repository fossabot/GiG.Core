# GiG.Core.Orleans.Client

This Library provides an API to register an Orleans Client in an application.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddClusterClient((x, sp) =>
    {
        x.ConfigureCluster(_configuration);              
        x.AddAssemblies(typeof(IGrain));
    });
}

```

### Configuration

The below table outlines the valid Configurations used to override the [ClusterOptions](https://github.com/dotnet/orleans/blob/master/src/Orleans.Core/Configuration/Options/ClusterOptions.cs) under the Config section `Orleans:Cluster`.

| Configuration Name | Type   | Optional | Default Value |
|:-------------------|:-------|:---------|:--------------|
| ClusterId          | String | Yes      | `dev`         |
| ServiceId          | String | Yes      | `dev`         |

## Correlation Id

Add the below to your Startup class to add CorrelationId. 
 
```csharp

      public void ConfigureServices(IServiceCollection services)
      {
         x.AddCorrelationOutgoingFilter(sp);
      }

```