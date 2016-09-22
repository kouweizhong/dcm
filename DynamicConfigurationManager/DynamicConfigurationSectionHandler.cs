using System.Configuration;
using System.Xml;

namespace DynamicConfigurationManager
{
    /// <summary>
    ///     Handles the access to the DynamicConfigurationSection configuration section.
    ///     <see cref="IConfigurationSectionHandler" /> is deprecated in .NET Framework 2.0 and above. But, because it
    ///     is used internally it is still supported.
    /// </summary>
    public class DynamicConfigurationSectionHandler : IConfigurationSectionHandler
    {
        /// <summary>
        ///     Creates a configuration section handler.
        /// </summary>
        /// <param name="parent">Parent object.</param>
        /// <param name="configContext">Configuration context object.</param>
        /// <param name="dynamicConfigurationSection">XML section as an XMLnode.</param>
        /// <returns>A NameValueCollection of new application settings.</returns>
        public object Create(object parent, object configContext, XmlNode dynamicConfigurationSection)
        {
            // Throw error if null section
            if (dynamicConfigurationSection == null)
            {
                throw new ConfigurationErrorsException("The 'DynamicConfigurationManagerSection' node is not found in app.config.");
            }

            var dynamicConfigAppsettings = new DynamicConfigurationBuilder().Build(dynamicConfigurationSection);

            return dynamicConfigAppsettings;
        }
    }
}