using GiG.Core.Data.Serializers.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace GiG.Core.Data.Serializers
{
    /// <summary>
    /// Xml Data Serializer.
    /// </summary>
    public class XmlDataSerializer : IXmlDataSerializer
    {
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly ConcurrentDictionary<string, XmlSerializer> _cache = new ConcurrentDictionary<string, XmlSerializer>();

        /// <inheritdoc/>
        public XmlDataSerializer()
        {
            _xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = false,
                NewLineHandling = NewLineHandling.None,
                NewLineOnAttributes = false,
                OmitXmlDeclaration = false
            };
        }

        /// <inheritdoc />
        public string Serialize(Type type, object data, XmlNamespaceManager xmlNamespaceManager, bool setDefaultNamespace = true)
        {
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();

            IDictionary<string, string> namespaces = xmlNamespaceManager?.GetNamespacesInScope(XmlNamespaceScope.Local);

            foreach (string prefix in namespaces?.Keys)
            {
                xmlSerializerNamespaces.Add(prefix, namespaces[prefix]);
            }

            using (var stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    var serializer = GetSerializer(type, setDefaultNamespace ? xmlNamespaceManager?.DefaultNamespace : null);
                    serializer.Serialize(writer, data, xmlSerializerNamespaces);

                    return stringWriter.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public string Serialize(Type type, object data)
        {
            var xmlSerializer = GetSerializer(type);

            using (StringWriter stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    xmlSerializer.Serialize(writer, data);

                    return stringWriter.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public object Deserialize(Type type, string data)
        {
            var xmlSerializer = GetSerializer(type);

            object o;

            using (TextReader textReader = new StringReader(data))
            {
                o = xmlSerializer.Deserialize(textReader);
            }

            return o;
        }

        private XmlSerializer GetSerializer(Type type, string defaultNamespace = "")
        {
            return _cache.GetOrAdd(defaultNamespace + "|" + type.FullName, keyString => new XmlSerializer(type, defaultNamespace));
        }
    }
}