using System.Collections.Specialized;
using System.Configuration;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Linq;

namespace DynamicConfigurationManager
{
    /// <summary>
    ///     DynamicConfigurationManager dnyamically creates a new configuration settings at runtime
    ///     based on configuration map logic.
    /// </summary>
    public static class DynamicConfigurationManager
    {
        private static readonly object LockObj = new object();

        public static NameValueCollection AppSettings
        {
            get
            {
                lock (LockObj)
                {
                    return (NameValueCollection) ConfigurationManager.GetSection("DynamicConfigurationSection");
                }
            }
        }

        /// <summary>
        ///     Get database connection string for environment from DynamicConfigurationManager
        /// </summary>
        public static string ConnectionString(string dynamicConnectionAliasKey, bool throwOnNotFound = false)
        {
            var cx = ConnectionStringSetting(dynamicConnectionAliasKey, throwOnNotFound);
            return cx?.ConnectionString;
        }

        /// <summary>
        ///     Get database connection settings for environment from DynamicConfigurationManager
        /// </summary>
        /// <param name="dynamicConnectionAliasKey"></param>
        /// <param name="throwOnNotFound"></param>
        /// <returns></returns>
        public static ConnectionStringSettings ConnectionStringSetting(string dynamicConnectionAliasKey,
            bool throwOnNotFound = false)
        {
            if (!throwOnNotFound && (AppSettings?[dynamicConnectionAliasKey] == null))
            {
                return null;
            }

            return ConfigurationManager.ConnectionStrings[AppSettings[dynamicConnectionAliasKey]];
        }
    }
}