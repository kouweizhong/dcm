using System.Collections.Specialized;
using System.Xml;

namespace DynamicConfigurationManager
{
    internal interface IConfigurationBuilder
    {
        NameValueCollection Build(XmlNode dynamicConfigurationSection);
    }
}