namespace DynamicConfigurationManager
{
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// DynamicConfigurationService dnyamically creates a new configuration settings at runtime
    /// based on configuration map logic.
    /// </summary>
    public static class DynamicConfigurationService
    {
        private static readonly object lockObj = new object();

        public static NameValueCollection AppSettings
        {
            get
            {
                lock (lockObj)
                {
                    return (NameValueCollection)ConfigurationManager.GetSection("DynamicConfigurationSection");
                }
            }
        }

        /// <summary>
        /// Get database connection string for environment from DynamicConfigurationManager
        /// </summary>
        public static string ConnectionString(string dynamicConnectionAliasKey, bool throwOnNotFound = false)
        {
            var cx = ConnectionStringSetting(dynamicConnectionAliasKey, throwOnNotFound);
            return cx == null ? null : cx.ConnectionString;
        }

        /// <summary>
        /// Get database connection settings for environment from DynamicConfigurationManager
        /// </summary>
        /// <param name="dynamicConnectionAliasKey"></param>
        /// <param name="throwOnNotFound"></param>
        /// <returns></returns>
        public static ConnectionStringSettings ConnectionStringSetting(string dynamicConnectionAliasKey, bool throwOnNotFound = false)
        {
            if (!throwOnNotFound && (AppSettings == null || AppSettings[dynamicConnectionAliasKey] == null))
            {
                return null;
            }

            return ConfigurationManager.ConnectionStrings[AppSettings[dynamicConnectionAliasKey]];
        }

        public static NameValueCollection GetSection(string xpath)
        {
            return (NameValueCollection)ConfigurationManager.GetSection(xpath);
        }

        public static NameValueCollection GetSection(XElement configXml, string xpath)
        {
            return GetSection(configXml.GetXmlDoc(), xpath);
        }

        /// <summary>
        /// Allows pre-loaded Xml configuration document
        /// </summary>
        public static NameValueCollection GetSection(XmlDocument doc, string xpath)
        {
            var handler = new DynamicConfigurationSectionHandler();

            // If user specified appSettings, use Dynamic Config instead
            if (xpath == null || xpath.ToUpper() == "APPSETTINGS")
            {
                xpath = "DynamicConfigurationSection";
            }

            // Get the section name as an XML node
            XmlNode configNode = doc.SelectSingleNode("configuration");
            XmlNode node = configNode.SelectSingleNode(xpath);

            // Use the DynamicConfigSectionHandler to parse the section
            return (NameValueCollection)handler.Create(null, null, node);
        }

        public static NameValueCollection GetSection(string configFile, string xpath)
        {
            var doc = new XmlDocument();
            var configNode = doc.SelectSingleNode("configuration");
            var node = configNode.SelectSingleNode(xpath);
            var handler = new DynamicConfigurationSectionHandler();

            // If user specified appSettings, use Dynamic Config instead
            if (xpath == null || xpath.ToUpper() == "APPSETTINGS")
            {
                xpath = "DynamicConfigurationSection";
            }

            // Open config file as XML
            doc.Load(configFile);

            // Use the DynamicConfigSectionHandler to parse the section
            return (NameValueCollection)handler.Create(null, null, node);
        }
    }
}