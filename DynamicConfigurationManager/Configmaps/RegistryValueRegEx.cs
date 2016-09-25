using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     RegistryValueRegEx compares a regular expression from the configuration attribute value to
    ///     the value found in the systems registry.
    /// </summary>
    internal class RegistryValueRegEx : IConfigMap
    {
        /// <summary>
        ///     A collection of registry key names.
        /// </summary>
        private static readonly Dictionary<string, string> Mappings = new Dictionary<string, string>
        {
            {
                "HKLM", "HKEY_LOCAL_MACHINE"
            },
            {
                "HKCU", "HKEY_CURRENT_USER"
            },
            {
                "HKCC", "HKEY_CURRENT_CONFIG"
            },
            {
                "HKCR", "HKEY_CLASSES_ROOT"
            },
            {
                "HKU", "HKEY_USERS"
            }
        };

        /// <summary>
        ///     Determines if the given configuration map attribute matches the value found in the registry.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool IsMatch(string configMapAttribute)
        {
            if (configMapAttribute.Length == 0) return false;
            var namevalue = configMapAttribute.Split("=".ToCharArray(), 2);
            if (namevalue.Length != 2)
            {
                return false;
            }

            var regPath = namevalue[0];
            var valueRegEx = namevalue[1];

            if (string.IsNullOrWhiteSpace(regPath) || string.IsNullOrWhiteSpace(valueRegEx))
            {
                return false;
            }

            Trace.TraceInformation($"RegistryValueRegEx: matching {regPath} to {valueRegEx}");

            regPath = Mappings.Aggregate(regPath, (current, mapping) => current.Replace(mapping.Key, mapping.Value));

            // get the key (parent of the name) and value name
            var lastSlash = regPath.LastIndexOf('\\');
            var regKey = regPath.Substring(0, lastSlash);
            var regName = regPath.Substring(lastSlash + 1);

            // get the registry value
            var val = Registry.GetValue(regKey, regName, null);
            Trace.TraceInformation($"RegistryValueRegEx: registry key {regKey} value {regName} is '{val}'");

            if (val == null) return false;
            var re = new Regex(valueRegEx, RegexOptions.IgnoreCase);
            return re.IsMatch(val.ToString());
        }
    }
}