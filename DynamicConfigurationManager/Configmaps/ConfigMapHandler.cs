using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class ConfigMapHandler
    {
        private readonly Dictionary<string, IConfigMapAttribute> _configMapAttributesList =
            new Dictionary<string, IConfigMapAttribute>(StringComparer.InvariantCultureIgnoreCase);

        public ConfigMapHandler()
        {
            // Load list with ConfigMap commands
            _configMapAttributesList.Add("assemblyPath", new AssemblyPath());
            _configMapAttributesList.Add("assemblyPathRegEx", new AssemblyPathRegEx());
            _configMapAttributesList.Add("callingTypeFullnameRegEx", new CallingTypeFullnameRegEx());
            //this line is legacy, leaving it in
            _configMapAttributesList.Add("commandLineArg", new CommandLineArgs());
            _configMapAttributesList.Add("commandLineArgs", new CommandLineArgs());
            _configMapAttributesList.Add("configPathRegEx", new ConfigPathRegEx());
            _configMapAttributesList.Add("currentDirectory", new CurrentDirectory());
            _configMapAttributesList.Add("currentDirectoryRegEx", new CurrentDirectoryRegEx());
            _configMapAttributesList.Add("environmentVariable", new EnvironmentVariable());
            _configMapAttributesList.Add("executablePath", new ExecutablePath());
            _configMapAttributesList.Add("executablePathRegEx", new ExecutablePathRegEx());
            _configMapAttributesList.Add("hostname", new HostnameList());
            // this is intentional to avoid human error of adding hostnames but not changing the attribute name
            _configMapAttributesList.Add("hostnameList", new HostnameList());
            _configMapAttributesList.Add("hostnameRegEx", new HostnameRegEx());
            _configMapAttributesList.Add("sitePathRegEx", new SitePathRegEx());
            _configMapAttributesList.Add("registryValueRegEx", new RegistryValueRegEx());
            //_configMapAttributesList.Add("updateLocation", new UpdateLocation());
            _configMapAttributesList.Add("webHostName", new WebHostName());
            _configMapAttributesList.Add("webServiceBinPath", new WebServiceBinPath());
            _configMapAttributesList.Add("webUrl", new WebUrl());
            _configMapAttributesList.Add("webUrlList", new WebUrlList());
            _configMapAttributesList.Add("webUrlRegEx", new WebUrlRegEx());
        }

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

                    IConfigMapAttribute cmd;
                    if (_configMapAttributesList.TryGetValue(attrib.Name, out cmd))
                    {
                        // Execute the ConfigMap command
                        Trace.TraceInformation("Testing command handler {0}: attributes: {1}", attrib.Name, attrib.Value);
                        rtnValue = cmd.Execute(attrib.Value);
                        Trace.TraceInformation("Returned: {0}", rtnValue);

                        // All configMap attributes must return true or the configMap is not a match
                        if (rtnValue == false)
                            break;
                    }
                }
            }
            return rtnValue;
        }
    }
}
