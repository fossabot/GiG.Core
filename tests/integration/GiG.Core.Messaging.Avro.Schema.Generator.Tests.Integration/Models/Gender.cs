﻿using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

 namespace GiG.Core.Messaging.Avro.Schema.Generator.Tests.Integration.Models
{
    [NamedSchema(doc: "Provides all supported genders")]
    public enum Gender
    {
        Unspecified = 0,
        Male = 1,
        Female = 2
    }
}
