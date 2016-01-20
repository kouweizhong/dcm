namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// CurrentDirectoryRegEx compares the attribute value to the fully qualified path of the
    /// current directory using RegEx.
    /// </summary>
    internal class CurrentDirectoryRegEx : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration attribute value matches the current directory of
        /// the executing application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
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