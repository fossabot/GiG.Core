# GiG.Core.Hosting

This Library provides an API to register hosting related functionailty to an application.


## Application Metadata Accessor

Add the below to your Startup class and this will register the application metadata accessor.


```csharp

public void ConfigureServices(IServiceCollection services)
{
	services.AddApplicationMetadataAccessor();
}

```

## Info Management Endpoint

Add the below to the Startup class to register an information endpoint. The defaule Url is '/actuator/info'. This is used to get application information 
such as Application name and version.

```csharp

public void ConfigureServices(IServiceCollection services)
{
	services.ConfigureInfoManagement(_configuration);
}


public void Configure(IApplicationBuilder app)
{           
	app.UseInfoManagement();           
}

```
