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
        private readonly static ConcurrentDictionary<string, XmlSerializer> _cache = new ConcurrentDictionary<string, XmlSerializer>();

        /// <inheritdoc/>
        public XmlDataSerializer()
        {
            _xmlSerializer = new XmlSerializer(typeof(T));
        }

        /// <inheritdoc />
        public string Serialize(T model, XmlNamespaceManager xmlNamespaceManager, Encoding encoding, bool setDefaultNamespace = true)
        {
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            
            IDictionary<string, string> namespaces = xmlNamespaceManager?.GetNamespacesInScope(XmlNamespaceScope.Local);
            
            foreach (string prefix in namespaces?.Keys)
            {
                xmlSerializerNamespaces.Add(prefix, namespaces[prefix]);
            }
            
            var xmlWriterSettings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding
            };
           
            using(var stringWriter = new StringWriter())
            {
                using (var writer = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    var serializer = GetSerializer( setDefaultNamespace ? xmlNamespaceManager?.DefaultNamespace : null);
                    serializer.Serialize(writer, model, xmlSerializerNamespaces);
                    
                    return stringWriter.ToString();
                }
            }
        }

        /// <inheritdoc/>
        public string Serialize(T model)
        {
            using (StringWriter stringWriter = new StringWriter())
            {
                _xmlSerializer.Serialize(stringWriter, model);
                
                return stringWriter.ToString();
            }
        }

        /// <inheritdoc/>
        public T Deserialize(string value)
        {
            object data;
            
            using (TextReader textReader = new StringReader(value))
            {
                data = _xmlSerializer.Deserialize(textReader);
            }
            
            return (T) data;
        }

        private XmlSerializer GetSerializer(string defaultNamespace)
        {
            string key = typeof(T).AssemblyQualifiedName + "|" + defaultNamespace;
            return _cache.GetOrAdd(key, keyString => new XmlSerializer(typeof(T), defaultNamespace));
        }
    }
}