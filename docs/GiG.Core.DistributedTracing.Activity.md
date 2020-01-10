# GiG.Core.DistributedTracing.Activity

This Library provides an API to register Distributed Tracing using System.Diagnostics.Activity.

## Basic Usage

The below code needs to be added to the `Program.cs`. This will register the Activity context accessor.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    services.AddActivityAccessor();
}


```