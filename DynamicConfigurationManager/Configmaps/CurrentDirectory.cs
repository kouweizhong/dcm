namespace DynamicConfigurationManager.ConfigMaps
{
    using System;
    using System.Diagnostics;
    using System.IO;

    /// <summary>
    /// CurrentDirectory compares the attribute value to the fully qualified path of the current directory.
    /// </summary>
    internal class CurrentDirectory : IConfigMap
    {
        /// <summary>
        /// Determine if there is a match of the given configuration map attribute to the current
        /// directory of the executing application.
        /// </summary>
        /// <param name="configMapAttribute">The current configMap element.</param>
        /// <returns>Return true if we found a match.</returns>
        public bool Execute(string configMapAttribute)
        {
            bool rtnValue = false;

            if (configMapAttribute.Length != 0)
            {
                Trace.TraceInformation("CurrentDirectory: matching to {0}", Environment.CurrentDirectory);
                string fullPath = Path.GetFullPath(configMapAttribute.TrimEnd('\\'));

                rtnValue = fullPath.Equals(Environment.CurrentDirectory, StringComparison.InvariantCultureIgnoreCase);
            }

            return rtnValue;
        }
    }
}