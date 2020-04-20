using GiG.Core.Data.Serializers.Abstractions;
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
    /// <typeparam name="T"></typeparam>
    public class XmlDataSerializer<T> : IXmlDataSerializer<T>
    {
        private readonly XmlSerializer _xmlSerializer;
        private readonly XmlWriterSettings _xmlWriterSettings;
        private readonly ConcurrentDictionary<string, XmlSerializer> _cache = new ConcurrentDictionary<string, XmlSerializer>();

        /// <inheritdoc/>
        public XmlDataSerializer()
        {
            _xmlSerializer = new XmlSerializer(typeof(T));

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
        public string Serialize(T data, XmlNamespaceManager xmlNamespaceManager, bool setDefaultNamespace = true)
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
                    var serializer = GetSerializer(setDefaultNamespace ? xmlNamespaceManager?.DefaultNamespace : null);
                    serializer.Serialize(writer, data, xmlSerializerNamespaces);

                    return stringWriter.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public string Serialize(T data)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, _xmlWriterSettings))
                {
                    _xmlSerializer.Serialize(writer, data);

                    return stringWriter.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public T Deserialize(string data)
        {
            object o;

            using (TextReader textReader = new StringReader(data))
            {
                o = _xmlSerializer.Deserialize(textReader);
            }

            return (T)o;
        }

        private XmlSerializer GetSerializer(string defaultNamespace)
        {
            return _cache.GetOrAdd(defaultNamespace, keyString => new XmlSerializer(typeof(T), defaultNamespace));
        }
    }
}