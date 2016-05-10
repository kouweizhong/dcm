using System.Xml;
using System.Xml.Linq;

namespace DynamicConfigurationManager
{
    /// <summary>
    ///     Converts an XmlNode to an XElement and back again. Some methods take XmlNode objects as
    ///     parameters. These methods also may contain properties and methods that return XmlNode
    ///     objects. However, it is more convenient to work with LINQ to XML instead of the classes in
    ///     System.Xml (XmlDocument, XmlNode, etc.)
    ///     Reference:
    ///     http://blogs.msdn.com/b/ericwhite/archive/2008/12/22/convert-xelement-to-xmlnode-and-convert-xmlnode-to-xelement.aspx
    /// </summary>
    public static class XmlExtensions
    {
        /// <summary>
        ///     Gets the root element of the XmlNode.
        /// </summary>
        /// <param name="node">An XmlNode.</param>
        /// <returns>An Xml element.</returns>
        public static XElement GetXElement(this XmlNode node)
        {
            var document = new XDocument();

            using (var xmlWriter = document.CreateWriter())
            {
                node.WriteTo(xmlWriter);
            }

            return document.Root;
        }

        /// <summary>
        ///     Gets an Xml document from the given Xml node.
        /// </summary>
        /// <param name="element">An Xml node.</param>
        /// <returns>An Xml document.</returns>
        public static XmlDocument GetXmlDoc(this XNode element)
        {
            var elementRoot = element.Document ?? element;

            using (var reader = elementRoot.CreateReader())
            {
                var xmlDoc = new XmlDocument();

                xmlDoc.Load(reader);

                return xmlDoc;
            }
        }

        /// <summary>
        ///     Gets an Xml node from the given Xml element.
        /// </summary>
        /// <param name="element">An Xml element.</param>
        /// <returns>An Xml node.</returns>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (var xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }
    }
}