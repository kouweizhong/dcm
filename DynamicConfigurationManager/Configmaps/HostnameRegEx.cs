using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     HostnameRegEx compares a regular expression from the configuration attribute value to the
    ///     name of the machine executing the application using RegEx.
    /// </summary>
    internal class HostnameRegEx : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration map attribute matches the machine name of the
        ///     executing the application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            var hostName = Environment.MachineName;

            Trace.TraceInformation($"Hostname: matching to {hostName}");

            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);

            return re.IsMatch(hostName);
        }
    }
}