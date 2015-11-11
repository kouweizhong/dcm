﻿using System.Collections.Specialized;
using System.Configuration;
using System.Dynamic;
using System.Xml;
using System.Xml.Linq;

namespace DynamicConfigurationManager
{
    public static class DynamicConfigurationService
    {
        static readonly object lockObj = new object();
        static readonly dynamic dynamicSetting = new DynamicSetting();

        private class DynamicSetting : DynamicObject
        {
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                result = null;
                var v = AppSettings[binder.Name];
                if (v != null)
                {
                    result = v;
                    return true;
                }

                return false;
            }
        }

        public static dynamic Setting
        {
            get { return dynamicSetting; }
        }

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
        ///  Get database connection settings for environment from DynamicConfigurationManager
        /// </summary>
        /// <param name="dynamicConnectionAliasKey"></param>
        /// <param name="throwOnNotFound"></param>
        /// <returns></returns>
        public static ConnectionStringSettings ConnectionStringSetting(string dynamicConnectionAliasKey, bool throwOnNotFound = false)
        {
            if (!throwOnNotFound && (AppSettings == null || AppSettings[dynamicConnectionAliasKey] == null))
                return null;
            return ConfigurationManager.ConnectionStrings[AppSettings[dynamicConnectionAliasKey]];
        }

        /// <summary>
        ///  Get database connection string for environment from DynamicConfigurationManager
        /// </summary>
        public static string ConnectionString(string dynamicConnectionAliasKey, bool throwOnNotFound = false)
        {
            var cx = ConnectionStringSetting(dynamicConnectionAliasKey, throwOnNotFound);
            return cx == null ? null : cx.ConnectionString;
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
            // If user specified appSettings, use Dynamic Config instead
            if (xpath == null || xpath.ToUpper() == "APPSETTINGS")
                xpath = "DynamicConfigurationSection";
            // Get the section name as an XML node
            XmlNode configNode = doc.SelectSingleNode("configuration");
            XmlNode node = configNode.SelectSingleNode(xpath);

            // Use the DynamicConfigSectionHandler to parse the section
            var handler = new DynamicConfigurationSectionHandler();
            return (NameValueCollection)handler.Create(null, null, node);
        }

        public static NameValueCollection GetSection(string configFile, string xpath)
        {
            // If user specified appSettings, use Dynamic Config instead
            if (xpath == null || xpath.ToUpper() == "APPSETTINGS")
                xpath = "DynamicConfigurationSection";

            // Open config file as XML
            var doc = new XmlDocument();
            doc.Load(configFile);

            // Get the section name as an XML node
            var configNode = doc.SelectSingleNode("configuration");
            var node = configNode.SelectSingleNode(xpath);

            // Use the DynamicConfigSectionHandler to parse the section
            var handler = new DynamicConfigurationSectionHandler();
            return (NameValueCollection)handler.Create(null, null, node);
        }
    }
}

