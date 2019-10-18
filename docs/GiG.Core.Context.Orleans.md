# GiG.Core.Context.Orleans

This Library provides an API to register the Request Context Accessor functionality for Orleans.

## Basic Usage - Client

Add the below to your Startup class to register the Request Context accessor. 
Note: If the client is a Web Api use the Request Context Accessor from [GiG.Core.Context.Web](../src/GiG.Core.Context.Web).

..
```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRequestContext();
	}

```

Add the below to your Startup class to register an Orleans Client with the Request Context Outgoing filter.

```csharp

      public void ConfigureServices(IServiceCollection services)
      {
          services.AddClusterClient((x, sp) =>
          {
			  x.AddRequestContextOutgoingFilter(sp); 
              x.ConfigureCluster(_configuration);              
              x.AddAssemblies(typeof(IGrain));
          });
      }

```

## Basic Usage - Silo

Add the below to your Startup class to register the Request Context accessor.


```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRequestContext();
	}

```