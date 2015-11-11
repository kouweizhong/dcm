namespace DynamicConfigurationManager
{
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// From http://blogs.msdn.com/b/ericwhite/archive/2008/12/22/convert-xelement-to-xmlnode-and-convert-xmlnode-to-xelement.aspx
    /// </summary>
    public static class XmlExtensions
    {
        ///<summary>
        ///</summary>
        ///<param name="node"></param>
        ///<returns></returns>
        public static XElement GetXElement(this XmlNode node)
        {
            var xDoc = new XDocument();
            using (XmlWriter xmlWriter = xDoc.CreateWriter())
                node.WriteTo(xmlWriter);
            return xDoc.Root;
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<returns></returns>
        public static XmlNode GetXmlNode(this XElement element)
        {
            using (XmlReader xmlReader = element.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlReader);
                return xmlDoc;
            }
        }

        ///<summary>
        ///</summary>
        ///<param name="element"></param>
        ///<returns></returns>
        public static XmlDocument GetXmlDoc(this XNode element)
        {
            var elementRoot = element.Document ?? element;
            using (XmlReader reader = elementRoot.CreateReader())
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                return xmlDoc;
            }
        }
    }
}