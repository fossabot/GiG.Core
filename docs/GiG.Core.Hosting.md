# GiG.Core.Hosting

This Library provides an API to register hosting related functionailty to an application.


## Basic Usage

Add the below to your Startup class and this will register the application metadata accessor.


```csharp

	public void ConfigureServices(IServiceCollection services)
	{
		services.AddApplicationMetadataAccessor();
	}

```