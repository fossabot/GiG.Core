# GiG.Core.Data.Serializers

This Library provides an API to register the required services needed by the different Data Serializers.

## Basic Usage

The below code needs to be added to the `Program.cs`. This will register the `IDataSerializer<T>`.

The `JsonDataSerializer<T>` can be used to serialize and deserialize Json Data.
The `XmlDataSerializer<T>` can be used to serialize and deserialize Xml Data.
 
```csharp
public void ConfigureServices(IServiceCollection services)
{
    //Adds Json Data Serialization functionality.
    services.AddSystemTextJsonDataSerializer();

    //Adds Xml Data Serialization functionality.
    services.AddXmlDataSerializer();
}
```