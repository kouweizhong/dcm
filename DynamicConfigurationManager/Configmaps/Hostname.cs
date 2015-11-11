using System;
using System.Diagnostics;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class Hostname : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            var hostName = Environment.MachineName;

            Trace.TraceInformation("Hostname: matching to {0}", hostName);
            return configMapAttribute.Equals(hostName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}