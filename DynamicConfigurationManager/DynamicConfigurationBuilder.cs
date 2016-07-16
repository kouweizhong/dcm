using System.Collections.Specialized;
using System.Xml;

namespace DynamicConfigurationManager
{
    internal class DynamicConfigurationBuilder : IConfigurationBuilder
    {
        public XmlNode DynamicConfigurationSection { get; set; }

        public NameValueCollection Build(XmlNode dynamicConfigurationSection)
        {
            // Preserve the original section
            DynamicConfigurationSection = dynamicConfigurationSection;

            // A collection to store application settings
            var dynamicConfigurationAppsettings = new NameValueCollection();

            // Step 1 - GlobalSettings

            // Step 2 - Configuration Maps

            return dynamicConfigurationAppsettings;
        }
    }
}