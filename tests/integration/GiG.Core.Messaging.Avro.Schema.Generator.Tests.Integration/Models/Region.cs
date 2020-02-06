﻿using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

 namespace GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration.Models
{
    [NamedSchema("Represents a Region")]
    public partial class Region
    {
        [Field(nameof(Name))]
        public string Name { get; set; }
    }
}
