﻿using Confluent.Kafka;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Extensions
{
    /// <summary>
    /// Producer builder extension methods.
    /// </summary>
    public static class ProducerBuilderExtensions
    {
        public static ProducerBuilder<TKey, TValue> SetValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> builder, Serializers<TKey, TValue> serializers)
        {
            if (serializers.ValueSerializer != null)
            {
                builder.SetValueSerializer(serializers.ValueSerializer);
            }
            else if (serializers.AsyncValueSerializer != null)
            {
                builder.SetValueSerializer(serializers.AsyncValueSerializer);
            }

            return builder;
        }
    }
}