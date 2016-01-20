namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// ExecutablePathRegEx compares the attribute value to the fully qualified path of the
    /// AppDomain's current base directory using RegEx.
    /// </summary>
    internal class ExecutablePathRegEx : IConfigMap
    {
        /// <summary>
        /// Determines if the given configuration attribute matches the applications base directory
        /// using a regular expression.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
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