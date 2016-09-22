using System.Xml;

namespace DynamicConfigurationManager
{
    internal interface IConfigMapHandler
    {
        /// <summary>
        ///     Determines if we have a handler that can parse the configMap.
        /// </summary>
        /// <param name="configMap">The configMap XML element from the configuration section.</param>
        /// <returns>True if we found a handler to parse the configMap.</returns>
        bool IsHandled(XmlNode configMap);
    }
}