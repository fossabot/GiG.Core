﻿using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

namespace GiG.Core.Messaging.Avro.Sample.Models
{
    [NamedSchema("Provides all supported genders")]
    public enum Gender
    {
        Unspecified = 0,
        Male = 1,
        Female = 2
    }
}
