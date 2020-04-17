using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace GiG.Core.Data.Tests.Unit.Helpers
{
    public static class XmlHelper
    {
        private static ConcurrentDictionary<Type, XmlNamespaceManager> _namespaces = new ConcurrentDictionary<Type, XmlNamespaceManager>();

        public static XmlNamespaceManager GetXmlNamespaceManager<T>()
        {
            var namespaces = _namespaces.GetValueOrDefault(typeof(T));

            if (namespaces == default(XmlNamespaceManager))
            {
                XmlDocument doc = new XmlDocument();
                XmlNamespaceManager xnm = new XmlNamespaceManager(doc.NameTable);

                var listOfNamespaces = GetAllNamespacesFromObject(typeof(T)).ToList();
                for (int i = 0; i < listOfNamespaces.Count; i++)
                {
                    xnm.AddNamespace($"ns{i}", listOfNamespaces[i]);
                }
                _namespaces.TryAdd(typeof(T), xnm);
                namespaces = xnm;
            }

            return namespaces;
        }

        private static IEnumerable<string> GetAllNamespacesFromObject(Type objectType)
        {
            var namespaces = new List<string>();

            var xmlRootAttribute = objectType.GetCustomAttribute<XmlRootAttribute>();

            if (xmlRootAttribute != null)
            {
                namespaces.Add(xmlRootAttribute.Namespace);
            }

            PropertyInfo[] props = objectType.GetProperties();
            
            foreach (PropertyInfo prop in props)
            {
                var attribute = prop.GetCustomAttribute<XmlElementAttribute>();
                
                if (attribute != null)
                {
                    if (!prop.PropertyType.IsPrimitive)
                    {
                        namespaces.AddRange(GetAllNamespacesFromObject(prop.GetType()));
                    }

                    string urlNamespace = attribute.Namespace;
                    namespaces.Add(urlNamespace);
                }
            }

            return namespaces.Distinct();
        }
    }
}
