namespace DynamicConfigurationManager.ConfigMaps
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;
    using Microsoft.Win32;

    /// <summary>
    /// RegistryValueRegEx compares a regular expression from the configuration attribute value to
    /// the value found in the systems registry.
    /// </summary>
    internal class RegistryValueRegEx : IConfigMap
    {
        /// <summary>
        /// A collection of registry key names.
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
        /// Determines if the given configuration map attribute matches the value found in the registry.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                string[] namevalue = configMapAttribute.Split("=".ToCharArray(), 2);
                if (namevalue.Length != 2)
                {
                    return false;
                }

                string regPath = namevalue[0];
                string valueRegEx = namevalue[1];

                if (string.IsNullOrWhiteSpace(regPath) || string.IsNullOrWhiteSpace(valueRegEx))
                {
                    return false;
                }

                Trace.TraceInformation("RegistryValueRegEx: matching {0} to {1}", regPath, valueRegEx);

                foreach (var mapping in Mappings)
                {
                    regPath = regPath.Replace(mapping.Key, mapping.Value);
                }

                // get the key (parent of the name) and value name
                int lastSlash = regPath.LastIndexOf('\\');
                string regKey = regPath.Substring(0, lastSlash);
                string regName = regPath.Substring(lastSlash + 1);

                // get the registry value
                object val = Registry.GetValue(regKey, regName, null);
                Trace.TraceInformation("RegistryValueRegEx: registry key {0} value {1} is '{2}'", regKey, regName, val);

                if (val != null)
                {
                    var re = new Regex(valueRegEx, RegexOptions.IgnoreCase);
                    rtnValue = re.IsMatch(val.ToString());
                }
            }

            return rtnValue;
        }
    }
}