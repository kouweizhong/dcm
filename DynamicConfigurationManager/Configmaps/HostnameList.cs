namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Linq;

    /// <summary>
    /// HostnameList compares a list of hostname from the configuration attribute value to the name
    /// of the machine executing the application. Useful if your application runs in a multi-host
    /// production farm.
    /// </summary>
    internal class HostnameList : IConfigMap
    {
        /// <summary>
        /// Determines if one of the given list of hostnames from the configuration map attribute
        /// matches the machine name executing the application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
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