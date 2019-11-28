using Confluent.Kafka;
using System.Collections.Generic;

namespace GiG.Core.Messaging.Kafka.Abstractions.Extensions
{
    /// <summary>
    /// Dictionary extension methods.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Add or Update a Dictionary item.
        /// </summary>
        /// <param name="dictionary">The <see cref="T:IDictionary{TKey, TValue}"/>.</param>
        /// <param name="key">The key value of the dictionary item to add.</param>
        /// <param name="value">The value of the dictionary item to add.</param>
        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
            }
            else
            {
                dictionary.Add(key, value);
            }
        }

        /// <summary>
        /// Convert the <see cref="T:IDictionary{string, string}"/> to a <see cref="Confluent.Kafka.Headers"/> object.
        /// </summary>
        /// <param name="dictionary">The <see cref="T:IDictionary{string, string}"/>.</param>
        /// <returns>The <see cref="Confluent.Kafka.Headers"/>.</returns>
        public static Headers ToKafkaHeaders(this IDictionary<string, string> dictionary)
        {
            var headers = new Headers();

            // Add headers
            if (dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    headers.Add(kvp.Key, kvp.Value);
                }
            }

            return headers;
        }
    }
}