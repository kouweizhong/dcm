using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     ConfigPathRegEx compares the attribute value to the fully qualified path of the current
    ///     configuration file using RegEx.
    /// </summary>
    internal class ConfigPathRegEx : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration map attributes match the path to the current
        ///     configuration file using regular expressions.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            var currentConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            Trace.WriteLine("ConfigPathRegEx: matching to {0}", currentConfigPath);
            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
            return re.IsMatch(currentConfigPath);
        }
    }
}