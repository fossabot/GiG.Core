# GiG.Core.Logging.RabbitMQ

This Library provides an API to register Logging to RabbitMQ using Serilog for an application.

## Basic Usage

Make use of `ConfigureLogging(x => x.WriteToRabbitMQ())` when creating an `IHostBuilder`. Logging requires configuration.

```csharp

static class Program
{
    public static void Main()
    {
        CreateHostBuilder().Build().Run();
    }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host
            .CreateDefaultBuilder()
            .ConfigureLogging(x => x.WriteToRabbitMQ());
    }
}

```

### Configuration

The below table outlines the valid Configurations used to override the [RabbitMQSinkOptions](../src/GiG.Core.Logging.Sinks.RabbitMQ/Internal/RabbitMQSinkOptions.cs) under the Config section `Logging:Sinks:RabbitMQ`

| Configuration Name | Type   | Optional | Default Value |
|:-------------------|:-------|:---------|:--------------|
| Hostname           | string | Yes      | 'localhost'   |
| Username           | string | Yes      | 'guest'       |
| Password           | string | Yes      | 'guest'       |
| Port               | int    | Yes      | 5672          |
| Exchange           | string | Yes      | 'Logging'     |
| ExchangeType       | string | Yes      | 'direct'      |
| DeliveryMode       | string | Yes      | 'NonDurable'  |
| BatchPostingLimit  | int    | Yes      | 5             |
| Period             | int    | Yes      | 5             |
| MinimumLevel       | string | Yes      | 'Information' |
| Heartbeat          | int    | Yes      | 5             |
| VHost              | string | Yes      | '/'           |

#### Sample Configuration

```json
 {
   "Logging": {
     "MinimumLevel": "Debug",
     "Sinks": {
      "RabbitMQ": {
        "IsEnabled": true, 
        "Hostname": "localhost",
        "Port": "5672",
		"Exchange":"Logging",
		"ExchangeType":"direct",
		"Username":"guest",
		"Password":"guest",
		"VHost":"/",
		"BatchPostingLimit":5,
		"PeriodInSeconds":5,
		"DeliveryMode":"NonDurable",
		"Heartbeat":30
      }
     }
   }
 }
```