# GiG.Core.Context.Orleans

This Library provides an API to register the Request Context Accessor functionality for Orleans.

## Basic Usage - Client

The below code needs to be added to the `Startup.cs`. This will register the Request Context accessor. 

**Note**: If the client is a Web Api use the Request Context Accessor from [GiG.Core.Context.Web](../src/GiG.Core.Context.Web).

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRequestContext();
}
```

The below code needs to be added to the `Startup.cs`. This will register an Orleans Client with the Request Context Outgoing filter.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddDefaultClusterClient((x, sp) =>
    {
        x.AddRequestContextOutgoingFilter(sp); 
        x.ConfigureCluster(_configuration);              
        x.AddAssemblies(typeof(IGrain));
    });
}
```

## Basic Usage - Silo

The below code needs to be added to the `Startup.cs`. This will register the Request Context accessor.

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddRequestContext();
}
```