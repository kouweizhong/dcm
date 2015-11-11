using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DynamicConfigurationManager.ConfigMaps
{
    /// <summary>
    ///     CurrentDirectoryRegEx compares the attribute value to the fully qualified path of the current directory.
    /// </summary>
    internal class CurrentDirectoryRegEx : IConfigMapAttribute
    {
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                Trace.TraceInformation("CurrentDirectoryRegEx: matching to {0}", Environment.CurrentDirectory);
                var re = new Regex(configMapAttribute, RegexOptions.IgnoreCase);
                rtnValue = re.IsMatch(Environment.CurrentDirectory);
            }

            return rtnValue;
        }
    }
}