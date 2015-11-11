using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    internal class HostnameRegEx : IConfigMap
    {
        public bool Execute(string configMapAttribute)
        {
            var hostName = Environment.MachineName;

            Trace.TraceInformation("Hostname: matching to {0}", hostName);
            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
            return re.IsMatch(hostName);
        }
    }
}