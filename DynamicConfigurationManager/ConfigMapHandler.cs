using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using DynamicConfigurationManager.ConfigMaps;

namespace DynamicConfigurationManager
{
    /// <summary>
    ///     Loads all configuration map handlers and determines if a handler can process a configMap attribute.
    ///     Refer to: https://en.wikipedia.org/wiki/Chain-of-responsibility_pattern
    /// </summary>
    internal class ConfigMapHandler : IConfigMapHandler
    {
        /// <summary>
        ///     A dictionary that stores the list of configuration map handlers.
        /// </summary>
        private readonly Dictionary<string, IConfigMap> _configMapHandlers =
            new Dictionary<string, IConfigMap>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfigMapHandler" /> class.
        /// </summary>
        public ConfigMapHandler()
        {
            _configMapHandlers.Add("assemblyPath", new AssemblyPath());
            _configMapHandlers.Add("assemblyPathRegEx", new AssemblyPathRegEx());
            _configMapHandlers.Add("commandLineArgs", new CommandLineArgs());
            _configMapHandlers.Add("configPathRegEx", new ConfigPathRegEx());
            _configMapHandlers.Add("currentDirectory", new CurrentDirectory());
            _configMapHandlers.Add("currentDirectoryRegEx", new CurrentDirectoryRegEx());
            _configMapHandlers.Add("environmentVariable", new EnvironmentVariable());
            _configMapHandlers.Add("executablePath", new ExecutablePath());
            _configMapHandlers.Add("executablePathRegEx", new ExecutablePathRegEx());
            _configMapHandlers.Add("hostname", new HostnameList());
            _configMapHandlers.Add("hostnameList", new HostnameList());
            _configMapHandlers.Add("hostnameRegEx", new HostnameRegEx());
            _configMapHandlers.Add("sitePathRegEx", new SitePathRegEx());
            _configMapHandlers.Add("registryValueRegEx", new RegistryValueRegEx());
            _configMapHandlers.Add("webHostName", new WebHostName());
            _configMapHandlers.Add("webServiceBinPath", new WebServiceBinPath());
            _configMapHandlers.Add("webUrl", new WebUrl());
            _configMapHandlers.Add("webUrlList", new WebUrlList());
            _configMapHandlers.Add("webUrlRegEx", new WebUrlRegEx());
        }

        /// <summary>
        ///     Determines if we have a handler that can parse the configMap.
        /// </summary>
        /// <param name="configMap">The configMap XML element from the configuration section.</param>
        /// <returns>True if we found a handler to parse the configMap.</returns>
        public bool IsHandled(XmlNode configMap)
        {
            var rtnValue = false;

            // ConfigMap element can have multiple attributes
            if (configMap.Attributes == null) return false;

            foreach (XmlAttribute attrib in configMap.Attributes)
            {
                if (attrib.Name.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                {
                    Trace.TraceInformation($"Evaluating: {attrib.Value}");
                    continue;
                }

                IConfigMap cmd;
                if (!_configMapHandlers.TryGetValue(attrib.Name, out cmd)) continue;
                // IsMatch the ConfigMap command
                Trace.TraceInformation($"Testing command handler {attrib.Name}: attributes: {attrib.Value}");
                rtnValue = cmd.IsMatch(attrib.Value);
                Trace.TraceInformation($"Returned: {rtnValue}");

                // All configMap attributes must return true or the configMap is not a match
                if (rtnValue == false)
                {
                    break;
                }
            }

            return rtnValue;
        }
    }
}