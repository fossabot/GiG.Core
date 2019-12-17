using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;
using System;
using System.Collections.Generic;

namespace GiG.Core.Messaging.Avro.Sample.Models
{
    [NamedSchema(doc: "Represents a person")]
    public partial class Person
    {
        [Field(nameof(Email), "Email")]
        public string Email { get; set; }

        [Field(nameof(MyTimeSpan), "TimeSpan")]
        public TimeSpan MyTimeSpan { get; set; }

        [Field(nameof(Wage), "Wage per month")]
        public decimal? Wage { get; set; }

        [Field(nameof(MyByte), "My byte")]
        public byte? MyByte { get; set; }

        [Field(nameof(ContactsEnumerable), "An Enumerable of Contracts")]
        public IEnumerable<Address> ContactsEnumerable { get; set; }

        [Field(nameof(ContactsArray), "An Array of Contracts")]
        public Address[] ContactsArray { get; set; }

        [Field(nameof(ContactsList), "A List of Contracts")]
        public IList<Address> ContactsList { get; set; }

        [Field(nameof(ContactsReadOnlyList), "A Read Only List of Contracts")]
        public IReadOnlyList<Address> ContactsReadOnlyList { get; set; }
        
        [Field(nameof(MyBoolean), "A Boolean")]
        public bool? MyBoolean { get; set; }

        [Field(nameof(MyDouble), "A Double")]
        public double? MyDouble { get; set; }
        
        [Field(nameof(MySingle), "Single")]
        public float? MySingle { get; set; }
        
        [Field(nameof(MyLong))]
        public Int64? MyLong { get; set; }
        
        [Field(nameof(Id))]
        public Guid? Id { get; set; }

        [Field(nameof(Address), "The Address Object")]
        public Address Address { get; set; }

        [Field(nameof(Gender), defaultValue: Gender.Unspecified)]
        public Gender Gender { get; set; }

        [Field(nameof(Age), "Person's age", -1)]
        public int? Age { get; set; }

        [Field(nameof(Fullname))]
        public string Fullname { get; set; }

        [Field(nameof(DateOfBirth))]
        public DateTime? DateOfBirth { get; set; }
        
        [Field(nameof(Data))]
        public byte[] Data { get; set; }

        [Field(nameof(CreatedOn))]
        public DateTimeOffset? CreatedOn { get; set; }
    }
}
