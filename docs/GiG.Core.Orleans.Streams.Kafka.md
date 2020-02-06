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
  services.AddDefaultClusterClient((builder, sp) =>
  {
    builder
    .AddKafka("KafkaProvider")
    .WithOptions(options =>
    {
      options.FromConfiguration(ctx.Configuration);
      options.AddTopic("MyTopic");
    })
    .AddJson()
    .Build();
  });
}  

```
### Silo Startup

```csharp
private static void ConfigureOrleans(HostBuilderContext ctx, ISiloBuilder builder)
{
  builder
    .AddMemoryGrainStorage("PubSubStore")
    .AddKafka(Constants.StreamProviderName)
    .WithOptions(kafkaOptions =>
    {
      kafkaOptions.FromConfiguration(ctx.Configuration);
      kafkaOptions.ConsumeMode = ConsumeMode.StreamStart;
      kafkaOptions.AddTopic("MyTopic");
    })
    .AddJson()
    .Build();
}      
```

### Configuration

You can change the default value for the Kafka configuration by overriding the [KafkaOptions](../src/GiG.Core.Orleans.Streams.Kafka/Configurations/KafkaOptions.cs) by adding the following configuration settings under section `Orleans:Streams:Kafka`. The Brokers option is an array which is delimited with ';'.

| Configuration Name | Type     | Optional | Default Value    |
|:-------------------|:---------|:---------|:-----------------|
| Brokers            | String[] | No       | `localhost:9092` |
| ConsumerGroupId    | String   | No       | `null`           |

#### Sample Configuration

```json
{
  "Orleans": {    
    "Streams": {
      "Kafka": {
        "Brokers": "localhost:9092",
        "ConsumerGroupId": "OrleansStreamsBenchmark"
      }
    }
  }
}
```
