using System.IO;
using System.Reflection;

namespace GiG.Core.Messaging.Avro.Schema.Generator.Templates
{
    internal static class TemplateManager
    {
        private static readonly string ResourceNamespace;

        static TemplateManager() => ResourceNamespace = typeof(TemplateManager).Namespace;

        internal static string AvroClassTemplate => ReadFileResource("AvroClassTemplate.txt");

        private static string ReadFileResource(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            using (var stream = assembly.GetManifestResourceStream($"{ResourceNamespace}.{resourceName}"))
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}