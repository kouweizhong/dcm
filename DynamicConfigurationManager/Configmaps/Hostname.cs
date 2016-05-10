using System;
using System.Diagnostics;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     Hostname compares the configuration attribute value to the name of the machine executing the application.
    /// </summary>
    internal class Hostname : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration attribuge value matches the name of the machine
        ///     executing the application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            var hostName = Environment.MachineName;

            Trace.TraceInformation($"Hostname: matching to {hostName}");

            return configMapAttribute.Equals(hostName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}