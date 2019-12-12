using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GiG.Core.Messaging.Avro.Schema.Generator.MSBuild.Extensions
{
    internal static class XmlExtensions
    {
        internal static string FormatXml(this string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = "\r\n",
                OmitXmlDeclaration = true
            };

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}