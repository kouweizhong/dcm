using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     CurrentDirectoryRegEx compares the attribute value to the fully qualified path of the
    ///     current directory using RegEx.
    /// </summary>
    internal class CurrentDirectoryRegEx : IConfigMap
    {
        /// <summary>
        ///     Determines if the given configuration attribute value matches the current directory of
        ///     the executing application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            Trace.TraceInformation($"CurrentDirectoryRegEx: matching to {Environment.CurrentDirectory}");
            var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);

            return re.IsMatch(Environment.CurrentDirectory);
        }
    }
}