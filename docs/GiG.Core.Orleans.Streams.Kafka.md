# GiG.Core.Orleans.Streams

This Library provides an API to register Kafka options from configuration.

## Basic Usage

### Startup

The below code needs to be added to the `Startup.cs` to use this extension.

```csharp
		
private static void ConfigureServices(Microsoft.Extensions.Hosting.HostBuilderContext ctx, IServiceCollection services)
{
    // Orleans Client
    services.AddClusterClient((builder, sp) =>
    {
        builder.UseLocalhostClustering()
        .AddAssemblies(typeof(ISMSProviderProducerGrain))
        .AddSimpleMessageStreamProvider(Constants.SMSProviderName)
        .AddKafka(Constants.KafkaProviderName)
        .WithOptions(options =>
        {
            options.FromConfiguration(ctx.Configuration);
            options.ConsumeMode = ConsumeMode.StreamStart;

            options
                .AddTopic(Constants.MessageNamespace);
        })
        .AddJson()
        .Build();
    });
}
              
```

### Configuration

You can change the default value for the Kafka configuration by overriding the [KafkaOptions](..\src\GiG.Core.Orleans.Streams.Kafka\Configurations\KafkaOptions.cs) by adding the following configuration settings under section `Orleans:Streams:Kafka`. The Brokers option is an array which is delimited with ';'.

| Configuration Name  | Type	 | Optional | Default Value	   |
|---------------------|--------- |----------|------------------|
| Brokers			  | String[] | No	    | `localhost:9092` |
| ConsumerGroupId	  | String   | No	    | `null`           |	