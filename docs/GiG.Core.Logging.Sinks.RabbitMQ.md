# GiG.Core.Logging.Sinks.RabbitMQ

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

| Configuration Name | Type    | Optional | Default Value          |
|:-------------------|:--------|:---------|:-----------------------|
| IsEnabled          | Boolean | Yes      | false                  |
| Hostname           | String  | Yes      | `localhost`            |
| Username           | String  | Yes      | `guest`                |
| Password           | String  | Yes      | `guest`                |
| Port               | Int     | Yes      | 5672                   |
| Exchange           | String  | Yes      | `Logging`              |
| ExchangeType       | String  | Yes      | `direct`               |
| DeliveryMode       | String  | Yes      | `NonDurable`           |
| BatchPostingLimit  | Int     | Yes      | 5                      |
| Period             | Int     | Yes      | 5                      |
| Heartbeat          | Int     | Yes      | 5                      |
| VHost              | String  | Yes      | `/`                    |
| Ssl:IsEnabled      | Boolean | Yes      | false                  |
| Ssl:CertPath       | String  | Yes      | <null>                 |
| Ssl:CertPassphrase | String  | Yes      | <null>                 |
| Ssl:ServerName     | String  | Yes      | Fallback to `Hostname` |

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
        "Exchange": "Logging",
        "ExchangeType": "direct",
        "Username": "guest",
        "Password": "guest",
        "VHost": "/",
        "BatchPostingLimit": 5,
        "PeriodInSeconds": 5,
        "DeliveryMode": "NonDurable",
        "Heartbeat": 30
      }
     }
   }
 }
```