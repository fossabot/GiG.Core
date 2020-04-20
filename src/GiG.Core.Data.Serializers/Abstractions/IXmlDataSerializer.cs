using System;
using System.Xml;

namespace GiG.Core.Data.Serializers.Abstractions
{
    /// <summary>
    /// The Xml Data Serializer.
    /// </summary>
    public interface IXmlDataSerializer
    {
        /// <summary>
        /// Returns an object from a string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The <see cref="string"/>.</param>
        object Deserialize(Type type, string data);


        /// <summary>
        /// Converts an object to a string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        string Serialize(Type type, object data);

        /// <summary>
        /// Converts an object to a string.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="data">The data.</param>
        /// <param name="xmlNamespaceManager">The Xml Namespace Manager.</param>
        /// <param name="setDefaultNamespace">Determines whether the default namespace should be used in serialization.</param>
        string Serialize(Type type, object data, XmlNamespaceManager xmlNamespaceManager, bool setDefaultNamespace = true);
    }
}