﻿ using GiG.Core.Messaging.Avro.Schema.Abstractions.Annotations;

namespace GiG.Core.Messaging.Avro.Sample.Models
{
    [NamedSchema("Provides all supported countries")]
    public enum Country
    {
        Unspecified = 0,
        Malta = 1,
        Gozo = 2, 
        Comino = 3
    }
}
