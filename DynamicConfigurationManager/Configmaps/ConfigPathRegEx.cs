using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     ConfigPathRegEx compares the attribute value to the fully qualified path of the current configuration file.
    /// </summary>
    internal class ConfigPathRegEx : IConfigMapAttribute
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                string currentConfigPath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

                Trace.WriteLine("ConfigPathRegEx: matching to {0}", currentConfigPath);
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                rtnValue = re.IsMatch(currentConfigPath);
            }

            return rtnValue;
        }
    }
}