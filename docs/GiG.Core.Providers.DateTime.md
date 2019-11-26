# GiG.Core.Providers.DateTime

This Library provides an API to register different types of Date Time providers. These can be easily mocked if needed for testing purposes.

## Basic Usage

The below code needs to be added to the `Startup.cs`. This will register the default date time provider which is UTC.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddDateTimeProvider();
}

```

Other Date Time providers can be registered such as below

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.TryAddSingleton<IDateTimeProvider, LocalDateTimeProvider>();
}

```