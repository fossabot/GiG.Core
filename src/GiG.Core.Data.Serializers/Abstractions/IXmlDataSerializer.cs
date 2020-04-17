using System.Xml;

namespace GiG.Core.Data.Serializers.Abstractions
{
    /// <summary>
    /// The Xml Data Serializer.
    /// </summary>
    public interface IXmlDataSerializer<T> : IDataSerializer<T>
    {
        /// <summary>
        /// Converts data to a string.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="xmlNamespaceManager">The Xml Namespace Manager.</param>
        /// <param name="setDefaultNamespace">Determines whether the default namespace should be used in serialization.</param>
        string Serialize(T data, XmlNamespaceManager xmlNamespaceManager, bool setDefaultNamespace = true);
    }
}