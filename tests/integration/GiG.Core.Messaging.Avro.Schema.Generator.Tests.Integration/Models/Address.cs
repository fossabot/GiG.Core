using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration.Models
{
    [NamedSchema("Represents an address")]
    public partial class Address
    {
        [Field(nameof(AddressLine1))]
        public string AddressLine1 { get; set; }

        [Field(nameof(AddressLine2))]
        public string AddressLine2 { get; set; }

        [Field(nameof(City))]
        public string City { get; set; }

        [Field(nameof(Country), defaultValue: Country.Unspecified)]
        public Country Country { get; set; }

        [Field(nameof(Region))]
        public Region Region { get; set; }
    }
}
