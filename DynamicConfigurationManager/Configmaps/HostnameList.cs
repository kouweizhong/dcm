using System;
using System.Linq;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class HostnameList : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            // Loop through each hostname in the array
            return
                configMapAttribute.Split(',')
                    .Any(
                        host => Environment.MachineName.Equals(host.Trim(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}