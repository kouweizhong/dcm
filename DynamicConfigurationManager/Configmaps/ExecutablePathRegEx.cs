using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    /// ExecutablePathRegEx compares the attribute value to the fully qualified path of the
    /// AppDomain's current base directory.
    /// </summary>
    internal class ExecutablePathRegEx : IConfigMapAttribute
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                Trace.TraceInformation("ExecutablePathRegEx: matching to {0}", baseDirectory);
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                rtnValue = re.IsMatch(baseDirectory);
            }

            return rtnValue;
        }
    }
}