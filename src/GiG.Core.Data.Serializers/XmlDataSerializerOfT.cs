using GiG.Core.Data.Serializers.Abstractions;
using System.Xml;

namespace GiG.Core.Data.Serializers
{
    /// <summary>
    /// Xml Data Serializer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlDataSerializer<T> : IXmlDataSerializer<T> 

    {
        private readonly IXmlDataSerializer _xmlDataSerializer;

        /// <summary>
        /// Constructor.
        /// </summary>
        public XmlDataSerializer()
        {
            _xmlDataSerializer = new XmlDataSerializer();
        }

        /// <inheritdoc/>
        public T Deserialize(string data)
        {
            return (T) _xmlDataSerializer.Deserialize(typeof(T), data);
        }

        /// <inheritdoc/>
        public string Serialize(T data, XmlNamespaceManager xmlNamespaceManager, bool setDefaultNamespace = true)
        {
            return _xmlDataSerializer.Serialize(typeof(T), data, xmlNamespaceManager, setDefaultNamespace);
        }

        /// <inheritdoc/>
        public string Serialize(T data)
        {
            return _xmlDataSerializer.Serialize(typeof(T), data);
        }
    }
}