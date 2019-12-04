# GiG.Core.Messaging.Kafka

This Library provides an API to register Kafka Producers, Consumers and their dependencies for an application.

## Producer Basic Usage

The below code needs to be added to the `Startup.cs` class. This will register the Producer together with the required Options.

```csharp

public void ConfigureServices(IServiceCollection services)
{
    // Configuration
    services.Configure<KafkaProviderOptions>(_configuration.GetSection(KafkaProviderOptions.DefaultSectionName));
    
    services.AddKafkaProducer<string, Person>(options => options
        .WithJson()
        .FromConfiguration(_configuration)
        .WithTopic("new-person-topic"));
}

```

To consume the Producer, inject the KafkaProducer<TKey, TValue>, and pass the message to the ProduceAsync method.

```csharp
public class PersonService
{
    private readonly IKafkaProducer<string, Person> _kafkaProducer;
    
    public PersonService(IKafkaProducer<string, Person> kafkaProducer)
    {
        _kafkaProducer = kafkaProducer ?? throw new ArgumentNullException(nameof(kafkaProducer));
    }

    public async Task CreatePerson()
    {
        var person = Person.Default;
        var messageId = Guid.NewGuid().ToString();
        
        // insert Person to DB if needed.

        var message = new KafkaMessage<string, Person>
        {
            Key = "person",
            Value = person,
            MessageId = messageId,
            MessageType = "Person.Created"
        };

        await _kafkaProducer.ProduceAsync(message);
    }
}

```

## Consumer Basic Usage

The below code can be added to the `Program.cs` class. In this case we are registering it as an IHosted service.
This will register the Consumer together with the required Options and dependencies.

```csharp

private static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.Configure<KafkaProviderOptions>(configuration.GetSection(KafkaProviderOptions.DefaultSectionName));

            services
                .AddKafkaConsumer<string, Person>(options => options
                    .WithJson()
                    .FromConfiguration(hostContext.Configuration)
                    .WithTopic("new-person-topic"))
                .AddHostedService<ConsumerService>();
        });

```

To use the Consumer, inject the IKafkaConsumer<TKey, TValue>, and use the Consume and Commit methods.

```csharp
public class ConsumerService : BackgroundService
{
    private readonly IKafkaConsumer<string, Person> _consumer;

    public ConsumerService(IKafkaConsumer<string, Person> consumer) => _consumer = consumer ?? throw new ArgumentNullException(nameof(consumer));

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        RunConsumer(cancellationToken);
        return Task.CompletedTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _kafkaConsumer.Dispose();
        return Task.CompletedTask;
    }

    private void RunConsumer(CancellationToken token = default)
    {
        var count = 0;

        try
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var message = _kafkaConsumer.Consume(token);
                    HandleMessage(message);

                    if (count++ % 10 == 0)
                    {
                        _kafkaConsumer.Commit(message);
                        count = 0;
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                }
            }
        }
        catch (OperationCanceledException)
        {
            _kafkaConsumer.Dispose();
        }
    }

    private void HandleMessage(IKafkaMessage<string, Person> message)
    {
        var serializedValue = JsonConvert.SerializeObject(message.Value);

        foreach (var (key, value) in message.Headers)
        {
            _logger.LogInformation("Header: {key}\tValue: {value}", key, value);
        }
    }
}

```

### Configuration

The below table outlines the valid Configurations used to override the [KafkaProviderOptions](..\src\GiG.Core.Messaging.Kafka.Abstractions\KafkaProviderOptions.cs) under the default Config section `EventProvider`.

| Configuration Name        | Type                          | Optional | Default Value               |
|:--------------------------|:------------------------------|:---------|:----------------------------|
| BootstrapServers          | string                        | Yes      | `http://localhost:9092`     |
| GroupId                   | string                        | Yes      | `default-group`             |
| Topic                     | string                        | Yes      | `default-topic`             |
| MessageTimeoutMs          | int                           | Yes      | `25000`                     |
| AutoOffsetReset           | AutoOffsetReset               | Yes      | `AutoOffsetReset.Latest`    |
| EnableAutoCommit          | bool                          | Yes      | `false`                     |
| SaslUsername              | string                        | Yes      |                             |
| SaslPassword              | string                        | Yes      |                             |
| SecurityProtocol          | SecurityProtocol              | Yes      | `SecurityProtocol.Plaintext`|
| SaslMechanism             | SaslMechanism                 | Yes      | `SaslMechanism.Plain`       |
| AdditionalConfiguration   | IDictionary<string, string>   | Yes      | `dev`                       |
| SchemaRegistry            | String                        | Yes      | `http://localhost:8081`     |