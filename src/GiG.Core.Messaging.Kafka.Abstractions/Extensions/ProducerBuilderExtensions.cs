﻿using Confluent.Kafka;
 using System;

 namespace GiG.Core.Messaging.Kafka.Abstractions.Extensions
{
    /// <summary>
    /// Producer builder extension methods.
    /// </summary>
    public static class ProducerBuilderExtensions
    {
        /// <summary>
        /// Sets the Message Value serializers.
        /// </summary>
        /// <param name="builder">The <see cref="ProducerBuilder{TKey, TValue}"/>.</param>
        /// <param name="serializers">The serializers to use.</param>
        /// <returns>The <see cref="ProducerBuilder{TKey, TValue}"/>.</returns>
        public static ProducerBuilder<TKey, TValue> SetValueSerializer<TKey, TValue>(this ProducerBuilder<TKey, TValue> builder, Serializers<TKey, TValue> serializers)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            
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