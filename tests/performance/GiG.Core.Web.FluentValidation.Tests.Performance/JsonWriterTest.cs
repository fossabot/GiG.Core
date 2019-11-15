using BenchmarkDotNet.Attributes;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace GiG.Core.Web.FluentValidation.Tests.Performance
{
    public class JsonWriterTest
    {
        [Benchmark]
        public void JsonWriter_SerializeObject()
        {
            var fluentResponse = new ValidationResponse() {  Errors = new Dictionary<string, List<string>>()};
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("errors");
                    fluentResponse.Errors.ToList().ForEach(kvp =>
                    {
                        writer.WriteStartArray(kvp.Key);
                        kvp.Value.ForEach(x => writer.WriteStringValue(x));
                        writer.WriteEndArray();
                    });

                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());
            }
        }


        [Benchmark]
        public void JsonWriter_SerializeObject_MediumSize()
        {
            var fluentResponse = new ValidationResponse() { Errors = new Dictionary<string, List<string>>() };
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test2", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test3", new List<string> { "dummy", "dummy2" });

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("errors");
                    fluentResponse.Errors.ToList().ForEach(kvp =>
                    {
                        writer.WriteStartArray(kvp.Key);
                        kvp.Value.ForEach(x => writer.WriteStringValue(x));
                        writer.WriteEndArray();
                    });

                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        [Benchmark]
        public void JsonWriter_SerializeObject_LargeSize()
        {
            var fluentResponse = new ValidationResponse() { Errors = new Dictionary<string, List<string>>() };
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test2", new List<string> { "dummy", "dummy2", "dummy3" });
            fluentResponse.Errors.Add("test3", new List<string> { "dummy", "dummy2", "dummy3", "dummy4" });
            fluentResponse.Errors.Add("test4", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test5", new List<string> { "dummy", "dummy2", "dummy3" });
            fluentResponse.Errors.Add("test6", new List<string> { "dummy", "dummy2", "dummy3", "dummy4" });

            using (var stream = new MemoryStream())
            {
                using (var writer = new Utf8JsonWriter(stream))
                {
                    writer.WriteStartObject();
                    writer.WriteStartObject("errors");
                    fluentResponse.Errors.ToList().ForEach(kvp =>
                    {
                        writer.WriteStartArray(kvp.Key);
                        kvp.Value.ForEach(x => writer.WriteStringValue(x));
                        writer.WriteEndArray();
                    });

                    writer.WriteEndObject();
                }

                var json = Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        [Benchmark]
        public void JsonSerializer_Serialize()
        {
            var fluentResponse = new ValidationResponse() { Errors = new Dictionary<string, List<string>>() };
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });

            JsonSerializer.Serialize(fluentResponse);
        }

        [Benchmark]
        public void JsonSerializer__MediumSize()
        {
            var fluentResponse = new ValidationResponse() { Errors = new Dictionary<string, List<string>>() };
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test2", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test3", new List<string> { "dummy", "dummy2" });

            JsonSerializer.Serialize(fluentResponse);
        }

        [Benchmark]
        public void JsonSerializer__LargeSize()
        {
            var fluentResponse = new ValidationResponse() { Errors = new Dictionary<string, List<string>>() };
            fluentResponse.Errors.Add("test", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test2", new List<string> { "dummy", "dummy2", "dummy3" });
            fluentResponse.Errors.Add("test3", new List<string> { "dummy", "dummy2", "dummy3", "dummy4" });
            fluentResponse.Errors.Add("test4", new List<string> { "dummy", "dummy2" });
            fluentResponse.Errors.Add("test5", new List<string> { "dummy", "dummy2", "dummy3" });
            fluentResponse.Errors.Add("test6", new List<string> { "dummy", "dummy2", "dummy3", "dummy4" });

            JsonSerializer.Serialize(fluentResponse);
        }
    }

    internal class ValidationResponse
    {
        /// <summary>
        /// Error Messages.
        /// </summary>
        public IDictionary<string, List<string>> Errors { get; set; }
    }

}
