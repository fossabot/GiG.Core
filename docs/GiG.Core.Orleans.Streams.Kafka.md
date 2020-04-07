# GiG.Core.Orleans.Streams.Kafka

This Library provides an API to register Kafka options from configuration.

## Basic Usage

### Client Startup

The below code needs to be added to the `Startup.cs` to use this extension.
**Note**: The `AddDefaultClusterClient` extension can be found in the nuget package ```GiG.Core.Orleans.Client```.

```csharp
private static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
{
// Orleans Client
    services
    .AddDefaultClusterClient((builder, sp) =>
    {
        builder.AddKafkaStreamProvider("KafkaProvider", kafkaBuilder =>
        {
            kafkaBuilder.WithOptions(options =>
            {
                options.FromConfiguration(ctx.Configuration);
                options.AddTopicStream("MyTopic", ctx.Configuration);                                
            })
            .AddJson();
        });
    });
}  

```
### Silo Startup

```csharp
private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
{
    builder
    .AddMemoryGrainStorage("PubSubStore")    
    .AddKafkaStreamProvider("KafkaProvider", kafkaBuilder =>
    {
        kafkaBuilder.WithOptions(options =>
        {
            options.FromConfiguration(ctx.Configuration);
            options.AddTopicStream("MyTopic", ctx.Configuration);
        })
        .AddJson();
    });
}      
```

### Configuration

You can change the default value for the Kafka configuration by overriding the [KafkaOptions](../src/GiG.Core.Orleans.Streams.Kafka/Configurations/KafkaOptions.cs) by adding the following configuration settings under section `Orleans:Streams:Kafka`. The Brokers option is an array which is delimited with ';'.

| Configuration Name           | Type     | Optional | Default Value                    |
|------------------------------|----------|----------|----------------------------------|
| Brokers                      | String[] | No       | `localhost:9092`                 |
| ConsumerGroupId              | String   | No       | `null`                           |
| Security:IsEnabled           | String   | Yes      | `false`                          |
| Security:SaslUsername        | String   | No       | `null`                           |
| Security:SaslPassword        | String   | No       | `null`                           |
| Security:SecurityProtocol    | String   | Yes      | `SecurityProtocol.SaslPlaintext` |
| Security:SaslMechanism       | String   | Yes      | `SaslMechanism.Plain`            |
| Topic:IsTopicCreationEnabled | Boolean  | Yes      | `true`          				 |
| Topic:Partitions             | Integer  | Yes      | 3                                |
| Topic:ReplicationFactor      | Short    | Yes      | 2                                |
| Topic:RetentionPeriodInMs    | Ulong    | Yes      | `null`                           |

When the Security section is enabled, both username and password are validated so they cannot be left empty.

When the Topic section is enabled through the 'IsTopicCreationEnabled' flag,  new topic streams will be set up with the configurations under the `Orleans:Streams:Kafka:Topic` section.
In case you require different settings for different Topics, you can add another section (for example 'Orleans:Streams:Kafka:Topic2') in the configuration file and include the configuration section name when calling 'AddTopicStream':

```csharp
   options.AddTopicStream("MyTopic2", configuration.GetSection("Orleans:Streams:Kafka:Topic2"));          
```
#### Sample Configuration

```json
{
  "Orleans": {    
    "Streams": {
      "Kafka": {
        "Brokers": "localhost:9092",
        "ConsumerGroupId": "OrleansStreamsBenchmark",
        "Security": {
          "IsEnabled": true,
          "SaslUsername": "user",
          "SaslPassword": "password"
        }, 
        "Topic": {
          "IsTopicCreationEnabled": true,
          "Partitions": "3",
          "ReplicationFactor": "2"
        },
      }
    }
  }
}
```
