﻿using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

namespace GiG.Core.Messaging.Avro.Sample.Models
{
    [NamedSchema("Represents a Region")]
    public partial class Region
    {
        [Field(nameof(Name))]
        public string Name { get; set; }
    }
}
