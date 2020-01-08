# GiG.Core.Messaging.Avro.Schema.Generator.MSBuild

This Library provides a Code Generator that generates the Avro schema for Public Events objects.

## Schema Generator Basic Usage

This code generation package leverages Roslyn both for code generation and code analysis. It does not load application binaries and as a result avoids issues caused by clashing dependency versions and differing target frameworks. 

This packages should be installed into all projects which contain Public.Events. Installing a package injects a target into the project which will generate code at build time.

## Usage
 - Identify the project containing Public.Events that need to be published to the Avro Schema Registry and Kafka.
 - Install package \[GiG.Core.Messaging.Avro.Schema.Abstractions]
 - Install package \[GiG.Core.Messaging.Avro.Schema.Generator.MSBuild]
 - Decorate classes with [NamedSchema(doc: \"")] and properties with [Field("")]
 - Build you project
 
### Example of generated code

Event model, including NamedSchema and Field attributes:
```csharp
[NamedSchema(doc: "Represents a person")]
public partial class Person
{
	[Field(nameof(Email))]
	public string Email { get; set; }

	[Field(nameof(Id))]
	public Guid Id { get; set; }

	[Field(nameof(Address))]
	public Address Address { get; set; }

	[Field(nameof(Gender), defaultValue: Gender.Unspecified)]
	public Gender Gender { get; set; }

	[Field(nameof(Age), "Person's age", -1)]
	public int? Age { get; set; }

	[Field(nameof(Fullname))]
	public string Fullname { get; set; }

	[Field(nameof(DateOfBirth))]
	public DateTime DateOfBirth { get; set; }
	
	[Field(nameof(Data))]
	public byte[] Data { get; set; }
}
```

**Generated code**

```csharp
public partial class Person : ISpecificRecord
{
	// ReSharper disable once InconsistentNaming
	// ReSharper disable once MemberCanBePrivate.Global
	// ReSharper disable once FieldCanBeMadeReadOnly.Global
	public static Schema _SCHEMA = Schema.Parse(@"{""type"":""record"",""name"":""Person"",""namespace"":""KafkaMessaging.Models"",""doc"":""Represents a person"",""fields"":[{""name"":""Email"",""type"":""string""},{""name"":""Id"",""type"":{""name"":""Id"",""type"":""fixed"",""size"":16}},{""name"":""Address"",""type"":{""type"":""record"",""name"":""Address"",""namespace"":""KafkaMessaging.Models"",""doc"":""Represents an address"",""fields"":[{""name"":""AddressLine1"",""type"":""string""},{""name"":""AddressLine2"",""type"":""string""},{""name"":""City"",""type"":""string""},{""name"":""Country"",""type"":{""name"":""Country"",""type"":""enum"",""symbols"":[""UNSPECIFIED"",""MALTA"",""GOZO"",""COMINO""]},""default"":""UNSPECIFIED""},{""name"":""Region"",""type"":{""type"":""record"",""name"":""Region"",""namespace"":""KafkaMessaging.Models"",""doc"":""Represents a Region"",""fields"":[{""name"":""Name"",""type"":""string""}]}}]}},{""name"":""Gender"",""type"":{""name"":""Gender"",""type"":""enum"",""symbols"":[""UNSPECIFIED"",""MALE"",""FEMALE""]},""default"":""UNSPECIFIED""},{""name"":""Age"",""type"":{""type"":""array"",""items"":""int""}},{""name"":""Fullname"",""type"":""string""},{""name"":""DateOfBirth"",""type"":""long"",""logicalType"":""timestamp-millis""},{""name"":""Data"",""type"":""bytes""}]}");

	public object Get(int fieldPos)
	{
		switch (fieldPos)
		{
			case 0: return Email;
			case 1: return Id.ToString();
			case 2: return Address;
			case 3: return SerializeEnum(Gender);
			case 4: return Age.GetValueOrDefault();
			case 5: return Fullname;
			case 6: return SerializeDateTime(DateOfBirth);
			case 7: return SerializeBytes(Data);

			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
		}
	}

	public void Put(int fieldPos, object fieldValue)
	{
		switch (fieldPos)
		{
			case 0: Email = (string)fieldValue; break;
			case 1: Id = Guid.Parse((string)fieldValue); break;
			case 2: Address = (KafkaMessaging.Models.Address)fieldValue; break;
			case 3: Gender = DeserializeEnum<KafkaMessaging.Models.Gender>(fieldValue); break;
			case 4: Age = (int?)fieldValue; break;
			case 5: Fullname = (string)fieldValue; break;
			case 6: DateOfBirth = DeserializeDateTime((long)fieldValue); break;
			case 7: Data = DeserializeBytes((byte[])fieldValue); break;

			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
		}
	}

	public Schema Schema => _SCHEMA;
	
	#region DateTime serialization helpers
	
	private static long SerializeDateTime(DateTime timestamp)
	{
		var dateTimeOffset = new DateTimeOffset(timestamp.ToUniversalTime());
		return dateTimeOffset.ToUnixTimeMilliseconds();
	}
	
	private static DateTime DeserializeDateTime(long timestamp)
	{
		var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timestamp);
		return dateTimeOffset.UtcDateTime;
	}
	
	#endregion
	
	#region Bytes serialization helpers

	private static readonly byte[] DefaultByteArray = new byte[0];
	
	private static byte[] SerializeBytes(byte[] data)
	{
		return data ?? DefaultByteArray;
	}
	
	private static byte[] DeserializeBytes(byte[] data)
	{
		return data?.Length > 0 ? data : null;
	}
	
	#endregion
	
	#region Enum serialization helpers
	
	private static string SerializeEnum(Enum value)
	{
		return value.ToString().ToUpperInvariant();
	}
	
	private static TModel DeserializeEnum<TModel>(object data)
	{
		var val = Enum.Parse(typeof(TModel), data.ToString());
		return (TModel) val;
	}
	
	#endregion
}
```