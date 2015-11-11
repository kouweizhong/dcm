namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml;

    /// <summary>
    /// Loads all configuration map handlers and determines if a handler can process a configMap attribute.
    /// </summary>
    internal class ConfigMapHandler
    {
        /// <summary>
        /// A dictionary that stores the list of configuration map handlers.
        /// </summary>
        private readonly Dictionary<string, IConfigMap> configMapHandlers =
            new Dictionary<string, IConfigMap>(StringComparer.InvariantCultureIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigMapHandler"/> class.
        /// </summary>
        public ConfigMapHandler()
        {
            configMapHandlers.Add("assemblyPath", new AssemblyPath());
            configMapHandlers.Add("assemblyPathRegEx", new AssemblyPathRegEx());
            configMapHandlers.Add("commandLineArg", new CommandLineArgs());
            configMapHandlers.Add("commandLineArgs", new CommandLineArgs());
            configMapHandlers.Add("configPathRegEx", new ConfigPathRegEx());
            configMapHandlers.Add("currentDirectory", new CurrentDirectory());
            configMapHandlers.Add("currentDirectoryRegEx", new CurrentDirectoryRegEx());
            configMapHandlers.Add("environmentVariable", new EnvironmentVariable());
            configMapHandlers.Add("executablePath", new ExecutablePath());
            configMapHandlers.Add("executablePathRegEx", new ExecutablePathRegEx());
            configMapHandlers.Add("hostname", new HostnameList());
            configMapHandlers.Add("hostnameList", new HostnameList());
            configMapHandlers.Add("hostnameRegEx", new HostnameRegEx());
            configMapHandlers.Add("sitePathRegEx", new SitePathRegEx());
            configMapHandlers.Add("registryValueRegEx", new RegistryValueRegEx());
            ////configMapHandlers.Add("updateLocation", new UpdateLocation());
            configMapHandlers.Add("webHostName", new WebHostName());
            configMapHandlers.Add("webServiceBinPath", new WebServiceBinPath());
            configMapHandlers.Add("webUrl", new WebUrl());
            configMapHandlers.Add("webUrlList", new WebUrlList());
            configMapHandlers.Add("webUrlRegEx", new WebUrlRegEx());
        }

        /// <summary>
        /// Determines if we have a handler that can parse the configMap.
        /// </summary>
        /// <param name="configMap">The configMap XML element from the configuration section.</param>
        /// <returns>True if we found a handler to parse the configMap.</returns>
        public bool IsHandled(XmlNode configMap)
        {
            bool rtnValue = false;

            // ConfigMap element can have multiple attributes
            if (configMap.Attributes != null)
            {
                foreach (XmlAttribute attrib in configMap.Attributes)
                {
                    if (attrib.Name.Equals("name", StringComparison.InvariantCultureIgnoreCase))
                    {
                        Trace.TraceInformation("Evaluating: {0}", attrib.Value);
                        continue;
                    }

                    IConfigMap cmd;
                    if (configMapHandlers.TryGetValue(attrib.Name, out cmd))
                    {
                        // Execute the ConfigMap command
                        Trace.TraceInformation("Testing command handler {0}: attributes: {1}", attrib.Name, attrib.Value);
                        rtnValue = cmd.Execute(attrib.Value);
                        Trace.TraceInformation("Returned: {0}", rtnValue);

                        // All configMap attributes must return true or the configMap is not a match
                        if (rtnValue == false)
                        {
                            break;
                        }
                    }
                }
            }

            return rtnValue;
        }
    }
}