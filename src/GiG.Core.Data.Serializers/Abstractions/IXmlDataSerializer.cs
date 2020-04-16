using System.Text;
using System.Xml;

namespace GiG.Core.Data.Serializers.Abstractions
{
    /// <summary>
    /// The Xml Data Serializer.
    /// </summary>
    public interface IXmlDataSerializer<T> : IDataSerializer<T>
    {
        /// <summary>
        /// Converts a model to a string.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="xmlNamespaceManager">The Xml Namespace Manager.</param>
        /// <param name="encoding">The Encoding.</param>
        /// <param name="setDefaultNamespace">Determines whether the default namespace should be used in serialization</param>
        string Serialize(T model, XmlNamespaceManager xmlNamespaceManager, Encoding encoding, bool setDefaultNamespace = true);
    }
}